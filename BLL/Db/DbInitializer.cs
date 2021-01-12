using Microsoft.EntityFrameworkCore;

namespace MyApp.Bll.Db
{
  public static class DbInitializer
  {
    public static void Initialize(MyAppContext db, bool wipeDatabase = false)
    {
      CreateSchema(db, wipeDatabase);
    }

    static void CreateSchema(MyAppContext db, bool wipeDatabase)
    {
      if (wipeDatabase)
        db.Database.EnsureDeleted();
    }
  }
}
