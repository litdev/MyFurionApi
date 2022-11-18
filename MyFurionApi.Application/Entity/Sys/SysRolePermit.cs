﻿namespace MyFurionApi.Application.Entity
{
    /// <summary>
    /// 用户组权限
    /// </summary>
    [SugarTable(tableName: null)]
    public class SysRolePermit : BaseEntity
    {
        /// <summary>
        /// 用户组Id
        /// </summary>
        [FsColumn("用户组Id")]
        public int RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [FsColumn("权限Id")]
        public int PermitId { get; set; }
    }
}