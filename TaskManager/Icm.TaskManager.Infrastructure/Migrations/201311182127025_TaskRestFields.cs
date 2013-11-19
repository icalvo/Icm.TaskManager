namespace Icm.TaskManager.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskRestFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "CreationDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "StartDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "DueDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "FinishDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "RepeatInterval", c => c.Time(precision: 7));
            AddColumn("dbo.Tasks", "RepeatFromDueDate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "Priority", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "AlarmDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "Notes", c => c.String());
            AddColumn("dbo.Tasks", "Labels", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Labels");
            DropColumn("dbo.Tasks", "Notes");
            DropColumn("dbo.Tasks", "AlarmDate");
            DropColumn("dbo.Tasks", "Priority");
            DropColumn("dbo.Tasks", "RepeatFromDueDate");
            DropColumn("dbo.Tasks", "RepeatInterval");
            DropColumn("dbo.Tasks", "FinishDate");
            DropColumn("dbo.Tasks", "DueDate");
            DropColumn("dbo.Tasks", "StartDate");
            DropColumn("dbo.Tasks", "CreationDate");
        }
    }
}
