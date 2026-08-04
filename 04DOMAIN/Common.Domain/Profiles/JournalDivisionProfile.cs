using AutoMapper;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Model.Base;
using Common.Domain.DTO.Model;
using DataModel.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Profiles
{
    public class JournalDivisionProfile : Profile
    {
        public JournalDivisionProfile() {
           
            CreateMap<JournalDivisionDTO, Transaction>().ForMember(des => des.AccountId, sou => sou.MapFrom(s=>new PFAID(s.AccountID).UID) ).ForMember(des => des.StoreId, sou => sou.MapFrom(s => new PFAID(s.StoreID).UID)).ForMember(des => des.Memo, sou => sou.MapFrom(s => s.Description)).ForMember(des => des.TargetId, sou => sou.MapFrom(s => new PFAID(s.TargetID).UID)).ForMember(des => des.Amount, sou => sou.MapFrom(s => s.Debit>0?s.Debit: s.Credit)).ForMember(des => des.TargetType, sou => sou.MapFrom(s => s.TargetType)).ForMember(des => des.Id, sou => sou.MapFrom(s => new PFAID(s.ID).UID));
            CreateMap<Transaction, JournalDivisionDTO>().ForMember(des => des.AccountID, sou => sou.MapFrom(s => new PFAID(s.AccountId).ToString())).ForMember(des => des.StoreID, sou => sou.MapFrom(s => new PFAID(s.StoreId).ToString())).ForMember(des => des.Description, sou => sou.MapFrom(s => s.Memo)).ForMember(des => des.TargetID, sou => sou.MapFrom(s => new PFAID(s.TargetId).ToString())).ForMember(des => des.Credit, sou => sou.Condition(s => s.DebitCredit==true)).ForMember(des => des.Debit, sou => sou.Condition(s => s.DebitCredit == false)).ForMember(des => des.TargetType, sou => sou.MapFrom(s => s.TargetType)).ForMember(des => des.ID, sou => sou.MapFrom(s => new PFAID(s.Id).ToString()));

        }

    }
}
