﻿namespace MyFurionApi.Application.Entity
{
    /// <summary>
    /// 功能下的权限
    /// </summary>
    [SugarTable(tableName: null)]
    public class SysPermit : BaseEntity
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        [FsColumn("权限名称", false, 50)]
        public string PermitName { get; set; }

        /// <summary>
        /// 权限别名
        /// </summary>
        [FsColumn("权限别名", false, 50)]
        public string AliasName { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [FsColumn("功能Id")]
        public int HandlerId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FsColumn("状态")]
        public bool Status { get; set; } = true;
    }
}