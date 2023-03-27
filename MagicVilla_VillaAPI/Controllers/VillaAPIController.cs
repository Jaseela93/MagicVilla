using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_VillaAPI.Controllers
{
    // [Route("api/[Controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        // private readonly ILogger<VillaAPIController> _logger;

        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

       // private readonly ILogging _logger;

        //public VillaAPIController(ILogging logger)
        //{
        //   // _logger = logger;
        //}

        public VillaAPIController(ApplicationDbContext db) 
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTo>> GetVillas() 
        {
            //_logger.Log("Getting All villas", "");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id:int}" , Name = "GetVilla")]
        // [ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        // [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTo> GetVilla(int id)
        {
            if(id==0) {
                //_logger.Log("Get Villa Error with ID " + id , "Error");
                return BadRequest(); 
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if(villa == null) { return NotFound(); }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTo> CreateVilla([FromBody]VillaDTo villaDTo)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTo.Name.ToLower()) != null)
                {
                ModelState.AddModelError("CustomError", "Villa already Exist !!");
                return BadRequest(ModelState);
            }

            if(villaDTo == null) 
            {
                return BadRequest(villaDTo);
            }
            if(villaDTo.Id >0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new()
            {
                Id = villaDTo.Id,
                Name = villaDTo.Name,
                Details= villaDTo.Details,
                Amenity= villaDTo.Amenity,
                ImageUrl= villaDTo.ImageUrl,
                Occupancy= villaDTo.Occupancy,
                Rate= villaDTo.Rate,
                Sqft= villaDTo.Sqft

            };
            villaDTo.Id= _db.Villas.OrderByDescending(u=>u.Id).FirstOrDefault().Id+1;
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { id = villaDTo.Id }, villaDTo);
            // return Ok(villaDTo);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u=>u.Id==id);
            if(villa== null) {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTo villaDTo)
        {
            if(villaDTo== null || id != villaDTo.Id)
            {
                return BadRequest();
            }
            //var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            //villa.Id = villaDTo.Id;
            //villa.Name = villaDTo.Name;
            //villa.Details = villaDTo.Details;
            //villa.Amenity = villaDTo.Amenity;
            //villa.ImageUrl = villaDTo.ImageUrl;
            //villa.Occupancy = villaDTo.Occupancy;
            //villa.Rate = villaDTo.Rate;
            //villa.Sqft = villaDTo.Sqft;

            Villa model = new()
            {
                Id = villaDTo.Id,
                Name = villaDTo.Name,
                Details = villaDTo.Details,
                Amenity = villaDTo.Amenity,
                ImageUrl = villaDTo.ImageUrl,
                Occupancy = villaDTo.Occupancy,
                Rate = villaDTo.Rate,
                Sqft = villaDTo.Sqft

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTo> patchDTo) 
        {
            if (patchDTo == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaDTo villaDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Amenity = villa.Amenity,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft

            };
            patchDTo.ApplyTo(villaDTO, ModelState);
            Villa model = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Amenity = villaDTO.Amenity,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return NoContent();
        }



    }
}
