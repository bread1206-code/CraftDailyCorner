public enum CreatorApplicationStatus : byte
{
    Pending = 0,	//待審核
    Approved = 1,	//已通過
    Rejected = 2,	//已拒絕
    Cancelled = 3	//申請人取消
}