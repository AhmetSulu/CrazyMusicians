using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch.Exceptions;

namespace CrazyMusicians.Controllers
{
    [Route("api/musicians")]
    [ApiController]
    public class MusiciansController : ControllerBase
    {
        private static List<Musician> _musicians = new List<Musician>
        {
            new Musician { Id = 1, Name = "Ahmet Çalgı", Profession = "Famous Instrument Player", FunFact = "Always plays wrong notes, but very entertaining" },
            new Musician { Id = 2, Name = "Zeynep Melodi", Profession = "Popular Melody Writer", FunFact = "Her songs are misunderstood, but very popular" },
            new Musician { Id = 3, Name = "Cemil Akor", Profession = "Crazy Chordist", FunFact = "Changes chords frequently, but surprisingly talented" },
            new Musician { Id = 4, Name = "Fatma Nota", Profession = "Surprise Note Producer", FunFact = "Always prepares surprises when producing notes" },
            new Musician { Id = 5, Name = "Hasan Ritim", Profession = "Rhythm Monster", FunFact = "Makes every rhythm in his own style, never fits but is funny" },
            new Musician { Id = 6, Name = "Elif Armoni", Profession = "Harmony Master", FunFact = "Sometimes plays harmonies wrong, but very creative" },
            new Musician { Id = 7, Name = "Ali Perde", Profession = "Tone Applicator", FunFact = "Plays each tone differently, always surprising" },
            new Musician { Id = 8, Name = "Ayşe Rezonans", Profession = "Resonance Specialist", FunFact = "Expert in resonance, but sometimes makes it too loud" },
            new Musician { Id = 9, Name = "Murat Ton", Profession = "Tone Enthusiast", FunFact = "Differences in toning are sometimes funny, but very interesting" },
            new Musician { Id = 10, Name = "Selin Akor", Profession = "Chord Magician", FunFact = "Sometimes creates a magical atmosphere when changing chords" }
        };

        // GET: api/musicians
        [HttpGet]
        public IEnumerable<Musician> GetAllMusicians()
        {
            return _musicians;
        }

        // GET: api/musicians/{id}
        [HttpGet("{id:int:min(1)}")]
        public ActionResult<Musician> GetMusicianById(int id)
        {
            var musician = _musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with Id {id} not found");
            }
            return Ok(musician);
        }

        // GET: api/musicians/search
        [HttpGet("search")]
        public ActionResult<IEnumerable<Musician>> SearchMusicians([FromQuery] string name)
        {
            var result = _musicians.Where(m => m.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase)).ToList();
            if (!result.Any())
            {
                return NotFound($"No musicians found with name {name}");
            }
            return Ok(result);
        }

        // POST: api/musicians
        [HttpPost]
        public ActionResult<Musician> AddMusician([FromBody] Musician musician)
        {
            musician.Id = _musicians.Max(m => m.Id) + 1;
            _musicians.Add(musician);
            return CreatedAtAction(nameof(GetMusicianById), new { id = musician.Id }, musician);
        }

        // PATCH: api/musicians/{id}
        [HttpPatch("{id:int:min(1)}")]
        public IActionResult UpdateMusician(int id, [FromBody] JsonPatchDocument<Musician> patchDoc)
        {
            var musician = _musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with Id {id} not found");
            }

            try
            {
                patchDoc.ApplyTo(musician, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }
            catch (JsonPatchException ex)
            {
                return BadRequest($"Invalid JSON Patch operation: {ex.Message}");
            }
            patchDoc.ApplyTo(musician);
            return NoContent();
        }

        // PUT: api/musicians/{id}
        [HttpPut("{id:int:min(1)}")]
        public IActionResult ReplaceMusician(int id, [FromBody] Musician updatedMusician)
        {
            var musician = _musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with Id {id} not found");
            }

            musician.Name = updatedMusician.Name;
            musician.Profession = updatedMusician.Profession;
            musician.FunFact = updatedMusician.FunFact;

            return NoContent();
        }

        // DELETE: api/musicians/{id}
        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteMusician(int id)
        {
            var musician = _musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with Id {id} not found");
            }

            _musicians.Remove(musician);
            return NoContent();
        }
    }
}
