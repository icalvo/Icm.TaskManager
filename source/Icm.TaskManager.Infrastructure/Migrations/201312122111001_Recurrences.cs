namespace Icm.TaskManager.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Recurrences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "RecurrenceType", c => c.String());
            AlterColumn("dbo.Tasks", "DueDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tasks", "RepeatFromDueDate");
        }

        public override void Down()
        {
            AddColumn("dbo.Tasks", "RepeatFromDueDate", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Tasks", "DueDate", c => c.DateTime());
            DropColumn("dbo.Tasks", "RecurrenceType");
        }
    }
}
