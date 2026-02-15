using System.ComponentModel.DataAnnotations;

public enum MemberRoleHistoryAction : byte
{
    [Display(Name = "建立")]
    Created = 0,	//建立
    [Display(Name = "更新")]
    Updata = 1,	//更新
    [Display(Name = "違規")]
    Violation =2	//違規
}