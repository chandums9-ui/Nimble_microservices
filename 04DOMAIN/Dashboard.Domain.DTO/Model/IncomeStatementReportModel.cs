using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.DTO.Model
{
    internal class IncomeStatementReportModel
    {
    }

    #region IncomeStatement Enums
    public enum IncomeStatementSetingEnum
    {
        [Display(Name = "FONT SETTINGS")]
        FontSettings,
        [Display(Name = "ADD ROW / SPACE")]
        AddRowOrSpace,
        [Display(Name = "APPLY FORMULAS")]
        ApplyFormulas,
        [Display(Name = "DELETE")]
        Delete,
    }

    public enum IS_FontSettingEnum
    {
        [Display(Name = "Font Size")]
        FontSize,
        [Display(Name = "Font Style")]
        FontStyle,
        [Display(Name = "Align Text")]
        AlignText,
        [Display(Name = "Align Data")]
        AlignData,
        [Display(Name = "Indent")]
        Indent,
        [Display(Name = "Display Line")]
        DisplayLine,
    }
    public enum IS_AddRowOrSpaceEnum
    {
        [Display(Name = "Section")]
        Section,
        [Display(Name = "Heading")]
        Heading,
        [Display(Name = "Insert Department/Group")]
        InsertDepartmentOrGroup,
        [Display(Name = "Insert Account/Stats")]
        InsertAccountOrStats,
        [Display(Name = "Apply Page Break")]
        ApplyPageBreak,
        [Display(Name = "Apply Line & Space")]
        ApplyLineAndSpace,
        [Display(Name = "Apply Space/Seperator")]
        ApplySpaceOrSeperator,
    }

    public enum IS_ApplyFormulasEnum
    {
        [Display(Name = "Apply Formula")]
        ApplyFormula,
        [Display(Name = "Apply % Income Formula")]
        ApplyPercentageIncome,
        [Display(Name = "Applt $ Change Formula")]
        ApplyDollarChange,
        [Display(Name = "COA Duplicate Rule")]
        COADuplicateRule,
    }
    public enum IS_DeleteEnum
    {
        [Display(Name = "Delete Row")]
        DeleteRow,
        //[Display(Name = "Delete All From Below")]
        //DeleteAllFromBelow,
    }

    public enum IS_FontStyleEnum
    {
        [Display(Name = "B")]
        Bold,
        [Display(Name = "I")]
        Italic,
        [Display(Name = "U")]
        UnderLine,
    }

    public enum IS_AlignTextEnum
    {
        [Display(Name = "Left")]
        Left,
        [Display(Name = "Middle")]
        Middle,
        [Display(Name = "Right")]
        Right,
    }
    public enum IS_AlignDateEnum
    {
        [Display(Name = "Left")]
        Left,
        [Display(Name = "Middle")]
        Middle,
        [Display(Name = "Right")]
        Right,
    }
    public enum IS_IndentTextEnum
    {
        [Display(Name = "Left")]
        Left,
        [Display(Name = "Right")]
        Right,
    }
    public enum IS_DisplayLineEnum
    {
        [Display(Name = "Hide")]
        Hide,
        [Display(Name = "Show")]
        Show,

    }
    public enum IS_AddRow_SectionEnum
    {
        [Display(Name = "Start")]
        Start,
        [Display(Name = "End")]
        End,
    }
    public enum IS_AddRow_HeadingEnum
    {
        [Display(Name = "Text")]
        Text,
        [Display(Name = "Total")]
        Total,
    }

    public enum IS_LayoutTypeEnum
    {
        [Display(Name = "Summery And Details")]
        SummeryAndDetails = 1,
        [Display(Name = "Details")]
        Details,
    }

    public enum IS_RowTypeConstantsEnum
    {
        [Display(Name ="S")]
        Section,
        [Display(Name = "D")]
        Department,
        [Display(Name = "G")]
        DepartmentGroup,
        [Display(Name = "A")]
        GroupAccount,
        [Display(Name = "#")]
        GroupStats,
        [Display(Name = "H")]
        Heading,
        [Display(Name = "T")]
        Total,
        [Display(Name = "U")]
        ApplyLineAndSpace,
        [Display(Name = "S")]
        SpaceOrSeparator,
        [Display(Name = "B")]
        PageBreak,
        [Display(Name = "S")]
        Start,
        [Display(Name = "E")]
        End,
        [Display(Name = "F")]
        Formula,
       
    }
    #endregion


    #region Incomestatement Models
    public class IS_DetailedModel
    {
        /// <summary>
        /// to assign section No, Department No, Heading No......
        /// </summary>
        public string RowNo { get; set; }


        /// <summary>
        /// to assign  auto increment serial no
        /// </summary>
        public int SNo { get; set; }
        /// <summary>
        /// to display  department name,group name or section name...
        /// for heading this property is editable
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// to display  department name,group name or section name...
        /// this property is readonly
        /// </summary>
        public string OriginalName { get; set; }


        public string PlaceHolder { get; set; }

        public string Type { get; set; }

        public string Amount { get; set; }

        public string PercIncome { get; set; }

        public string DollarChange { get; set; }

        public IS_SectionType? SectionType { get; set; } = null;

        public bool IsActiveRow { get; set; } = false;

        public int SectionRow { get; set; }

        public int Section_DeptRow { get; set; }

        public int SectionDeptGroup_Row { get; set; }

        public int Section_DeptGroup_Account_Row { get; set; }

        public int SectionTotalType { get; set; }//1=Department Total Heading 2=DepartmentGroup Total Heading


        public IS_LayoutFontModel FontSettings { get; set; }

        public string DeptId { get; set; }
        public Nullable<long> DeptGroupId { get; set; }

        public string DeptGroupAccountId { get; set; }

        public Nullable<bool> IsStats { get; set; }

        
    }

    public class IS_FontStyle
    {
        //public bool IsBold { get; set; }=false;
        //public bool IsItalic { get; set; } = false;
        //public bool IsUnderLine { get; set; } = false;

        public List<string> FontStyleList { get; set; }
    }
    public class IS_LayoutFontModel
    {
        public IS_LayoutFontModel() => (GUID) = (Guid.NewGuid());
        public int FontSize { get; set; }
        public List<string> FontStyle { get; set; }
        public int AlignIndentText { get; set; } = 0;
        public IS_AlignDateEnum? AlignData { get; set; } = null;
       
        public IS_AlignTextEnum AlignText { get; set; }
        public IS_DisplayLineEnum DisplayLine { get; set; }

        public Guid GUID { get; set; }
    }
    public class IS_ListIncomeStatementModel
    {
        public List<IS_DetailedModel> incomeStatementList { get; set; }

        public int SettingIndex { get; set; }

        public int? ChildSettingIndex { get; set; } = null;


    }

    /// <summary>
    /// 
    /// </summary>
    public enum IS_SectionType
    {
        SectionStart = 1,
        Department,
        HeadingText,
        HeadingTotal,
        DepartmentGroup,
        DepartmentGroupAccounts,
        GroupTotal,
        ApplyLineAndSpace,
        ApplySpaceOrSeparator,
        ApplyPageBreak,
        SectionEnd,
    }

    public class IS_BaseModel<T>
    {
        public T Data { get; set; }

    }
    public class IS_DepartmentModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }

        public bool IsInsert { get; set; }
        public List<IS_DeptGroups> DeptGroups { get; set; }
    }
    public class IS_DeptGroups_Accounts
    {

        public int DeptGroupId { get; set; }
        public string AccountId { get; set; }

        public string AccountName { get; set; }

        public Nullable<bool> IsStats { get; set; } = false;

        public string AccountType { get; set; } //Ex:Expense,Other Current Liability.....
    }
    public class IS_AccountOrStatsModel
    {

        public List<IS_DeptGroups_Accounts> AccountGroups { get; set; }
        public bool IsInsert { get; set; }

    }
    public class IS_DeptGroups
    {
        public long DeptGroupId { get; set; }

        public string DeptGroupName { get; set; }

        public List<IS_DeptGroups_Accounts> DeptGroupAccounts { get; set; }
    }

    

    public class IS_ApplyFormulaModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }

        public bool IsInsert { get; set; }
        public List<string> DeptGroup { get; set; }
    }
    public class IS_ApplyPercFormulaModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }

        public bool IsInsert { get; set; }
        public List<string> DeptGroup { get; set; }
    }
    public class IS_DollarChangeFormulaModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }

        public bool IsInsert { get; set; }
        public List<string> DeptGroup { get; set; }
    }
    public class IS_COADuplicateModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }

        public bool IsInsert { get; set; }
        public List<string> DeptGroup { get; set; }
    }
    //public class IS_ConstantMSG
    //{
    //    public const string Duplicate_SectionStart_ErrorMsg = "'SECTION START' can't create again.";
    //    public const string Duplicate_SectionEnd_ErrorMsg = "'SECTION END' can't create again.";
    //    public const string Section_UnSelect_ErrorMsg = "Please Select Any Display Name";
    //    public const string Section_ErrorMsg = "Unable to create this section.";
    //    public const string DepartmentGroup_Count_Error = "Please select any department group.";
    //    public const string SectionStart_Required_ErrorMsg = "Please create initially 'SECTION START' .";
    //}
    #endregion


    public class IncomeStatementInfo
    {
        public string IncomeStatementId { get; set; }

        [Required(ErrorMessage = "Report name is required.")]
        public string ReportName { get; set; }

        public string Notes { get; set; }

        [Required(ErrorMessage = "Layout name is required.")]
        public string LayoutName { get; set; }

        [Required(ErrorMessage = "Management group is required.")]
        public string ManagementGroup { get; set; }

        public string ManagementGroupText { get; set; }

        [Required(ErrorMessage = "Select atleast one corporation")]
        public List<string> CorpIds { get; set; }

        public int LayoutType { get; set; }

        public string LayoutTypeText { get; set; }
        public string CopyLayoutId { get; set; }

        public bool IsActive { get; set; } = true;
    }


    public class IS_CorporationList
    {
        public string CorporationID { get; set; }

        public string CorporationName { get; set; }

        public string LegalName { get; set; }

        public string PropertyType { get; set; }

        public string Service { get; set; }

        public string Brand { get; set; }

        public string PMS { get; set; }
    }

    public class IS_ChartOfAccounts
    {
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeID { get; set; }
        public int AccountTypeOrder { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountNumber { get; set; }
        /// <summary>
        /// Status =17 & AccountTypeID= 0x0FA700000000000000000000000000000024 => default account for UI
        /// </summary>
        public short AccountStatus { get; set; }

        public Nullable<bool> IsStats { get; set; }=false;
    }
    public class IS_ChartOfAccountRequest
    {
        public List<string> CorpIds { get; set; }

        public string AccountTypeID { get; set; }

    }
}
