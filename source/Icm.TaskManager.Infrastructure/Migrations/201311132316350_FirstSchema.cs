namespace Icm.TaskManager.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FirstSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Tasks");
        }
    }
}
