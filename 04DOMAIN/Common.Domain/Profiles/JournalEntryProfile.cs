using AutoMapper;
using Common.Domain.DTO.App;
using DataModel.Domain.DataModel;

using Common.Domain.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Profiles
{
    public class JournalEntryProfile: Profile
    {
        public JournalEntryProfile() {
            CreateMap<JournalEntryRequest, JournalEntry>().ForMember(des => des.EntryNumber, sou => sou.MapFrom(s => s.EntryNumber)).ForMember(des => des.EntryDate, sou => sou.MapFrom(s => s.EntryDate)).ForMember(des => des.CorporationId, sou => sou.MapFrom(s => new PFAID(s.CorpID).UID));
            CreateMap<JournalEntry,JournalEntryRequest>().ForMember(des => des.EntryNumber, sou => sou.MapFrom(s => s.EntryNumber)).ForMember(des => des.EntryDate, sou => sou.MapFrom(s => s.EntryDate)).ForMember(des => des.CorpID, sou => sou.MapFrom(s => new PFAID(s.CorporationId).ToString()));

        }
    }
   
}
