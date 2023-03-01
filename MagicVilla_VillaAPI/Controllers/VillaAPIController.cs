using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
   // [Route("api/[Controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTo>> GetVillas() 
        {
            return Ok(VillaStore.villaList);
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
            if(id==0) { return BadRequest(); }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
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

            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTo.Name.ToLower()) != null)
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

            villaDTo.Id=VillaStore.villaList.OrderByDescending(u=>u.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villaDTo);
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
            var villa = VillaStore.villaList.FirstOrDefault(u=>u.Id==id);
            if(villa== null) {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
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
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            
            villa.Name= villaDTo.Name;
            villa.Occupancy= villaDTo.Occupancy;
            villa.Sdft= villaDTo.Sdft;
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
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            patchDTo.ApplyTo(villa,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return NoContent();
        }



    }
}
