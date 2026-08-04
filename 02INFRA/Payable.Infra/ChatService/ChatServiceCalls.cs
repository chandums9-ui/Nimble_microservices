using Amazon.Runtime.Internal;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Payable.App.Contracts;
using Payable.Domain.DTO.Req;
using Payable.Domain.DTO.Resp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Payable.Infra.ChatService
{
    

    public class ChatServiceCalls:IChatService
    {
        #region Fields
        private readonly ChatURLs chaturl;

        #endregion Fields End

        #region Ctor
        public ChatServiceCalls(IOptions<ChatURLs> _chaturl)
        {
            this.chaturl=_chaturl.Value;
        }
        #endregion ctor

        #region Methods
        public async Task<JournalResponse> CreateChatRoom(string InputString,string RoomName)
        {
            JournalResponse response = new JournalResponse();
            string url = string.Concat(chaturl.MainChatURL, chaturl.CreateChatGrouptURL);
            using (var client = new RestClient(url))
            {
                var request = new RestRequest();
                request.AddBody(InputString);
                request.AddQueryParameter("room_name",RoomName);
                RestResponse resp = await client.ExecutePostAsync(request);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    response.Status = resp.Content;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    var errorResposne = JsonConvert.DeserializeObject<ChatErrorResponse>(resp.Content);
                    response.Status = errorResposne?.detail?.FirstOrDefault()?.msg ?? null;
                    response.StatusCode = StatusCodes.Status500InternalServerError; 
                }
            }
            return response;
        }
        public async Task<ChatUnreadCountResponse> GetMessageCounts(string UID)
        {
            ChatUnreadCountResponse response = new ChatUnreadCountResponse();
            string url = string.Concat(chaturl.MainChatURL, chaturl.UnreadMessagesCountURL);
            using (var client = new RestClient(url))
            {
                var request = new RestRequest();
                request.AddQueryParameter("user_id", UID);
                RestResponse resp = await client.ExecuteGetAsync(request);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    response.MessageCount = JsonConvert.DeserializeObject<List<UnreadCountData>>(resp.Content);
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            return response;
        }
        public async Task<GetRoomParticipants> GetUsersInRoom(string RoomID)
        {
            GetRoomParticipants response = new GetRoomParticipants();
            string url = string.Concat(chaturl.MainChatURL, chaturl.GetRoomParticipantsURL);
            using (var client = new RestClient(url))
            {
                var request = new RestRequest();
                request.AddQueryParameter("room_id", RoomID);
                RestResponse resp = await client.ExecuteGetAsync(request);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    response.participates = JsonConvert.DeserializeObject <List<ChatParticipants>>(resp.Content);
                    response.Status = response.participates!=null && response.participates.Any()?"":"Failed";
                    response.StatusCode = string.IsNullOrEmpty(response.Status)? StatusCodes.Status200OK: StatusCodes.Status500InternalServerError;

                }
                else
                {
                    response.Status = Constants.MSG_ENDPOINT_ERROR;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            return response;
        }
        public async Task<JournalResponse> AddUsersToRoom(string UserData,string RoomID)
        {
            JournalResponse response = new JournalResponse();
            string url = string.Concat(chaturl.MainChatURL, chaturl.AddUsersToGrouptURL);
            using (var client = new RestClient(url))
            {
                var request = new RestRequest();
                request.AddQueryParameter("room_id", RoomID);
                request.AddBody(UserData);
                RestResponse resp = await client.ExecutePutAsync(request);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    response.Status= resp.Content;
                    response.StatusCode = StatusCodes.Status200OK;

                }
                else
                {
                    response.Status = "The Given User/s Already Exist or Error in Input";
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            return response;
        }
        public async Task<JournalResponse> RemoveUsersFromRoom(string UserData, string RoomID)
        {
            JournalResponse response = new JournalResponse();
            string url = string.Concat(chaturl.MainChatURL, chaturl.RemoveUsersFromGrouptURL);
            using (var client = new RestClient(url))
            {
                var request = new RestRequest();
                request.AddQueryParameter("room_id", RoomID);
                request.AddBody(UserData);
                RestResponse resp = await client.ExecuteDeleteAsync(request);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    response.Status = resp.Content;
                    response.StatusCode = StatusCodes.Status200OK;

                }
                else
                {
                    response.Status = "The Given User/s Doesn't Exist or Error in Input";
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            return response;
        }
        
            #endregion Methods End
        }
}
