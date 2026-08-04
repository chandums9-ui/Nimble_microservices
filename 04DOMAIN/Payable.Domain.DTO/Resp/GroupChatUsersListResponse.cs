using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Domain.DTO.Resp
{
	public class GroupChatUsersListResponse
	{
		public List<GroupChatUsersList> UsersList { get; set; }
		public int StatusCode { get; set; }
		public string Status { get; set; }
	}

	public class GroupChatUsersList
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string ClientID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
	}
}
