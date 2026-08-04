using Common.Domain.DTO.App;
using Messages.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.Publishing
{
    public class PublishMapper
    {
        public static SynchMessage MapSynchMessage(short eventType, string ID, string CorpID, string ClientName, long urlID, string transferConnectionString)
        {
            SynchMessage synchMsg = new SynchMessage
            {
                EventType = eventType,
                ID = ID,
                CorpID = CorpID,
                ClientName = ClientName,
                UrlID = urlID,
                TransferConnectionString = transferConnectionString,
                CloneID = null,
                FromDate = null,
                ToDate = null,
                MessageID = new PFAID(new PFAID().UID).ToString(),
                IsUpdatePrevious = false 
            };
            return synchMsg;
        }

    }
}
