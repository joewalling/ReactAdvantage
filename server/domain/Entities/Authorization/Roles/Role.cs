using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactAdvantage.Domain.Entities.Authorization.Users;

namespace ReactAdvantage.Domain.Entities.Authorization.Roles
{
    /// <summary>
    ///     Represents a role in an application. A role is used to group permissions.
    /// </summary>
    /// <remarks>
    ///     Application should use permissions to check if user is granted to perform an operation.
    ///     Checking 'if a user has a role' is not possible until the role is static .
    ///     Static roles can be used in the code and can not be deleted by users.
    ///     Non-static (dynamic) roles can be added/removed by users and we can not know their name while coding.
    ///     A user can have multiple roles. Thus, user will have all permissions of all assigned roles.
    /// </remarks>
    public abstract class Role 
    {
        protected Role()
        {
            Name = Guid.NewGuid().ToString("N");

        }

        protected Role(int? tenantId, string displayName)
        {
            TenantId = tenantId;
            DisplayName = displayName;

        }

        public int? TenantId { get; set; }

        protected Role(int? tenantId, string name, string displayName)
            : this(tenantId, displayName)
        {
            Name = name;
        }

        /// <summary>
        ///     Unique name of this role.
        /// </summary>
        [Required]
        [StringLength(32)]
        public virtual string Name { get; set; }

        /// <summary>
        ///     Display name of this role.
        /// </summary>
        [Required]
        [StringLength(64)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        ///     Is this a static role?
        ///     Static roles can not be deleted, can not change their name.
        ///     They can be used programmatically.
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        ///     Is this role will be assigned to new users as default?
        /// </summary>
        public virtual bool IsDefault { get; set; }
        
        //TODO: Add
        /// <summary>
        ///     List of permissions of the role.
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }

        /// <summary>
        ///     Claims of this user.
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<RoleClaim> Claims { get; set; }

        /// <summary>
        ///     A random value that must change whenever a user is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual User DeleterUser { get; set; }

        public virtual User CreatorUser { get; set; }

        public virtual User LastModifierUser { get; set; }
        public object Id { get; private set; }

        public override string ToString()
        {
            return $"[Role {Id}, Name={Name}]";
        }

    }
}