using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.PagiantionExtension
{

    /*
    
      public class UserFilter
    {
        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public bool? Activeted { get; set; } = false;
        public bool? DeActiveted { get; set; }=false;

        public string? RoleName { get; set; } = null;

        public string? UserId { get; set; } = null;
    }



    private async Task<IQueryable<AppUser>> ApplyFiltersAsync(IQueryable<AppUser> qry, UserFilter? filter)
    {
        if (filter != null)
        {
            // Filter by StartDate
            if (filter.StartDate.HasValue)
            {
                qry = qry.Where(u => u.CreateDate != null && u.CreateDate >= filter.StartDate.Value);
            }

            // Filter by EndDate
            if (filter.EndDate.HasValue)
            {
                qry = qry.Where(u => u.CreateDate != null && u.CreateDate <= filter.EndDate.Value);
            }

            // Filter by UserId
            if (!string.IsNullOrEmpty(filter.UserId))
            {
                qry = qry.Where(u => u.Id.ToString() == filter.UserId);
            }

            // Filter by Activated
            if (filter.Activeted == true)
            {
                qry = qry.Where(u => u.IsActive == true);
            }

            // Filter by Deactivated
            if (filter.DeActiveted == true)
            {
                qry = qry.Where(u => u.IsActive == false);
            }

            // Role filtering needs to be handled after executing the query
            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                // Await the asynchronous call to get users in the specified role
                var usersInRole = await _userManager.GetUsersInRoleAsync(filter.RoleName);
                var userIds = usersInRole.Select(u => u.Id).ToList();

                // This part requires the query to be materialized (executed first)
                qry = qry.Where(u => userIds.Contains(u.Id));
            }
        }

        return qry;
    }   
    */

}
