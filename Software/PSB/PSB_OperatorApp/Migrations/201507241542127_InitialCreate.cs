namespace PSB_OperatorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actuals",
                c => new
                    {
                        ActualID = c.Int(nullable: false, identity: true),
                        Reference = c.String(maxLength: 100),
                        SerialNo = c.String(),
                        StartTimestamp = c.DateTime(nullable: false),
                        EndTimestamp = c.DateTime(),
                        PlanID = c.Int(),
                    })
                .PrimaryKey(t => t.ActualID)
                .ForeignKey("dbo.Plans", t => t.PlanID)
                .Index(t => t.PlanID);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        PlanID = c.Int(nullable: false, identity: true),
                        Reference = c.String(maxLength: 100),
                        Quantity = c.Int(nullable: false),
                        Operators = c.Int(nullable: false),
                        CreatedTimestamp = c.DateTime(),
                        StartTimestamp = c.DateTime(),
                        DT = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ShiftID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlanID)
                .ForeignKey("dbo.Shifts", t => t.ShiftID, cascadeDelete: true)
                .Index(t => t.ShiftID);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        ShiftID = c.Int(nullable: false, identity: true),
                        ShiftName = c.String(maxLength: 100),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ShiftID);
            
            CreateTable(
                "dbo.Breaks",
                c => new
                    {
                        BreakID = c.Int(nullable: false, identity: true),
                        ShiftID = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BreakID)
                .ForeignKey("dbo.Shifts", t => t.ShiftID, cascadeDelete: true)
                .Index(t => t.ShiftID);
            
            CreateTable(
                "dbo.ProductModels",
                c => new
                    {
                        ProductModelID = c.Int(nullable: false, identity: true),
                        Reference = c.String(maxLength: 100),
                        DT = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductModelID)
                .Index(t => t.Reference, unique: true, name: "ReferenceIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Breaks", "ShiftID", "dbo.Shifts");
            DropForeignKey("dbo.Plans", "ShiftID", "dbo.Shifts");
            DropForeignKey("dbo.Actuals", "PlanID", "dbo.Plans");
            DropIndex("dbo.ProductModels", "ReferenceIndex");
            DropIndex("dbo.Breaks", new[] { "ShiftID" });
            DropIndex("dbo.Plans", new[] { "ShiftID" });
            DropIndex("dbo.Actuals", new[] { "PlanID" });
            DropTable("dbo.ProductModels");
            DropTable("dbo.Breaks");
            DropTable("dbo.Shifts");
            DropTable("dbo.Plans");
            DropTable("dbo.Actuals");
        }
    }
}
