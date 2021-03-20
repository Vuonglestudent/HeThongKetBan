using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Job { get; set; }

        //
        public string Title { get; set; }
        public string FindPeople { get; set; }
        public string FindAgeGroup { get; set; }
        public string Summary { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public DateTime Dob { get; set; }

        /// <summary>
        /// Dưới đây là các thông số dùng để tính toán
        /// </summary>
        ///
        public List<UserFeature> Features { get; set; }
        public List<SearchFeature> SearchFeatures { get; set; }
    }
}
