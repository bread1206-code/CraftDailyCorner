using System.ComponentModel.DataAnnotations;

public enum ReviewStatus : byte
{
    [Display(Name = "顯示")]
    Visible = 0,	//顯示
    [Display(Name = "違規")]
    Violation = 1	//違規
}