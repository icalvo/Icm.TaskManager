namespace Icm.TaskManager.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reminders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reminders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlarmDate = c.DateTime(nullable: false),
                        Task_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Task_Id);
            
            DropColumn("dbo.Tasks", "AlarmDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "AlarmDate", c => c.DateTime());
            DropForeignKey("dbo.Reminders", "Task_Id", "dbo.Tasks");
            DropIndex("dbo.Reminders", new[] { "Task_Id" });
            DropTable("dbo.Reminders");
        }
    }
}
