using Data;
using EStudentsManager.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EStudentsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        /// <summary>
        /// Update address
        /// </summary>
        /// <param name="id">Student's ID to update address</param>
        /// <param name="removeAddress">If want to remove address from database if address has no students</param>
        /// <param name="addressToUpdate">Address's new data</param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public AddressToUpdate ModifyAddress([FromRoute]int id, [FromQuery] bool removeAddress, [FromBody] AddressToUpdate addressToUpdate)
        {
            var address = DataLayer.ModifyStudentAddress(id, removeAddress, addressToUpdate.City, addressToUpdate.Street, addressToUpdate.StreetNumber);
            return new AddressToUpdate
            {
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber
            };
        }
    }
}
