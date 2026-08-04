using Common.Domain.AuthDataModel;
using Common.Domain.Common;
using Common.Domain.DTO.App;
using Common.Domain.DTO.Enums;
using Common.Domain.DTO.Model;
using Dashboard.Domain.DataModel;
using Dashboard.Domain.DTO.Model;
using Dashboard.Domain.DTO.Req;
using Dashboard.Domain.DTO.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.Common
{
    public static class Mapper
    {
        public static WidgetFormula MapWidgetFormula(WidgetFormulaRequest Entity, WidgetFormula? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.ActualFormula = Entity.ActualFormula;
                origEntity.DisplayFormula = Entity.DisplayFormula;
                origEntity.Name = Entity.WidgetName;
                origEntity.Status = Entity.Status;
                origEntity.IsDefault = Entity.IsDefault;
                return origEntity;
            }
            else
            {
                WidgetFormula widgetFormula = new WidgetFormula
                {
                    Name = Entity.WidgetName,
                    DisplayFormula = Entity.DisplayFormula,
                    ActualFormula = Entity.ActualFormula,
                    IsDefault = Entity.IsDefault,
                    Status = Entity.Status,
                    
                   
                };

                return widgetFormula; 
                
            }
        }
        public static WidgetFormulaSettings MapFormulaSetings(WidgetSettingsFormulaDTO Enity, WidgetFormulaSettings? origEntity = null)
        {
            if (origEntity != null)
            {
                origEntity.FormulaId = Enity.FormulaID;
                origEntity.SourceType = Convert.ToBoolean(Enity.SourceType);
                origEntity.SourceName = Enity.SourceName;   
                origEntity.CorporationId = new PFAID(Enity.CorporationId).UID;
                origEntity.SortOrder= Enity.SortOrder;
                origEntity.GroupFor = Enity.GroupFor;
                origEntity.WidgetId= Enity.widgetId;
                origEntity.ClientId = !string.IsNullOrEmpty(Enity.ClientID) ? new PFAID(Enity.ClientID).UID : null;
                return origEntity;
            }
            else
            {
                return new WidgetFormulaSettings
                {
                    Id = Enity.Id,
                    FormulaId = Enity.FormulaID,
                    SourceType = Convert.ToBoolean(Enity.SourceType),
                    SourceName = Enity.SourceName,
                    CorporationId = new PFAID(Enity.CorporationId).UID,
                    SortOrder = Enity.SortOrder,
                    GroupFor = Enity.GroupFor,
                    WidgetId= Enity.widgetId,
                ClientId =!string.IsNullOrEmpty(Enity.ClientID)? new PFAID(Enity.ClientID).UID:null
            };
            }
        }
        public static PandLAccountsResponse MapToPandLAccountsResponse(IncomeGroupAnalysisDbResponse incomeGroupAnalysisDbResponse)
        {
            return new PandLAccountsResponse
            {
                CorporationID = incomeGroupAnalysisDbResponse.GroupID.ToString(),
                CorpDBAName = incomeGroupAnalysisDbResponse.DeptName, // You can modify this to map as needed
                CorpLegalName = incomeGroupAnalysisDbResponse.Group,  // You can modify this to map as needed
                Type = (Int16)incomeGroupAnalysisDbResponse.Type,
                SubType = (Int16)incomeGroupAnalysisDbResponse.Subtype,
                CustomKeyID = (Int16)incomeGroupAnalysisDbResponse.Type==1? incomeGroupAnalysisDbResponse.customkeyid.ToString():incomeGroupAnalysisDbResponse.DeptID.ToString(),   
                CustomKeyName = incomeGroupAnalysisDbResponse.DeptName, // Modify if necessary
                ActualValue = incomeGroupAnalysisDbResponse.balance,
                LYvalue = incomeGroupAnalysisDbResponse.LYBalance,
                BudgetValue = incomeGroupAnalysisDbResponse.BudBalance,
                ForecastValue = incomeGroupAnalysisDbResponse.ForecastBalance,
                ActualPerincome = incomeGroupAnalysisDbResponse.perincomeact,
                LYPerincome = incomeGroupAnalysisDbResponse.perincomeLY,
                budgetPerincome = incomeGroupAnalysisDbResponse.perincomebud,
                forecastPerincome = incomeGroupAnalysisDbResponse.perincomefr,
                ActualPOR = incomeGroupAnalysisDbResponse.Actualpor,
                LYPOR = incomeGroupAnalysisDbResponse.LYPOR,
                BudgetPOR = incomeGroupAnalysisDbResponse.BudgetPOR,
                ForecastPOR = incomeGroupAnalysisDbResponse.ForecastPOR,
                ActualPAR = incomeGroupAnalysisDbResponse.ActualPAR,
                LYPAR = incomeGroupAnalysisDbResponse.LYPAR,
                BudgetPAR = incomeGroupAnalysisDbResponse.BudgetPAR,
                ForecastPAR = incomeGroupAnalysisDbResponse.ForecastPAR,
                ActualSTATS = incomeGroupAnalysisDbResponse.STATSORHOURS,
                BudgetSTATS = incomeGroupAnalysisDbResponse.BUDSTATSorHOURS,
                ForecastSTATS= incomeGroupAnalysisDbResponse.FRSTATSorHOURS,
                LYSTATS= incomeGroupAnalysisDbResponse.lySTATSORHOURS,
                MonthName = incomeGroupAnalysisDbResponse.MonthORDayORYear, // Modify if necessary
                Operatingrevenue= incomeGroupAnalysisDbResponse.OperatingRevenue,
                Budrevenue= incomeGroupAnalysisDbResponse.Budrevenue,
                ForecastRevenue= incomeGroupAnalysisDbResponse.ForecastRevenue,
                LYrevenue=incomeGroupAnalysisDbResponse.LYrevenue,
                MonthORDay = "2023",// incomeGroupAnalysisDbResponse.MonthORDayORYear, // Modify if necessary
                CustomKey = incomeGroupAnalysisDbResponse.customkeyid // Modify if necessary
            };
        }


        #region WidgetPrivileges

        public static WidgetPrivileges MapWidgetPrivileges(WidgetPrivilegeDTO Entity, long urlInfoID, WidgetPrivileges? OriginEntity = null)
        {

            if (OriginEntity != null)
            {
                OriginEntity.IsAddWidget = Entity.IsAddwidget;
                OriginEntity.IsExport = Entity.IsExport;
                OriginEntity.IsSchedule = Entity.IsSchedule;
                OriginEntity.UrlInfoId= urlInfoID;
               // OriginEntity.UserInfoId=Entity.UserInfoID;
                OriginEntity.ClientInfoId=Entity.ClientInfoID;
                return OriginEntity;
            }
            else
            {
                OriginEntity = new WidgetPrivileges();
                OriginEntity.SourceId = new PFAID(Entity.SourceID).UID;
                OriginEntity.SourceType = Entity.SourceType;
                OriginEntity.IsAddWidget = Entity.IsAddwidget;
                OriginEntity.IsExport = Entity.IsExport;
                OriginEntity.IsSchedule = Entity.IsSchedule;
                // OriginEntity.UserInfoId = Entity.UserInfoID;
                // OriginEntity.ClientInfoId = Entity.ClientInfoID;
                OriginEntity.UrlInfoId = urlInfoID;
                return OriginEntity;
            }
        }

        public static WidgetPrivilegeDetails MapWidgetPrivilgeDetails(WidgetPrivilegeDetailsDTO Entity, long WidgetPrivID, WidgetPrivileges widpris, WidgetPrivilegeDetails? OriginEntity = null)
        {
            if (OriginEntity != null)
            {
                OriginEntity.IsView = Entity.IsEnabled;
                OriginEntity.IsNavigate = Entity.IsNavigate;
                OriginEntity.IsFormula = Entity.IsFormulaBuilder;
                OriginEntity.IsDelete = Entity.IsDelete;
                return OriginEntity;
            }
            else
            {
                OriginEntity = new WidgetPrivilegeDetails();
                OriginEntity.WidgetPrivId = WidgetPrivID;
                OriginEntity.WidgetPriv = widpris;
                OriginEntity.WidgetId = Entity.WidgetID;
                OriginEntity.IsView = Entity.IsEnabled;
                OriginEntity.IsNavigate = Entity.IsNavigate;
                OriginEntity.IsFormula = Entity.IsFormulaBuilder;
                OriginEntity.IsDelete = Entity.IsDelete;
                return OriginEntity;
            }
        }

        #endregion
    }
}

