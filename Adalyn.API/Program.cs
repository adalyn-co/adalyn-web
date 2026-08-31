using Adalyn.API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("IzinVer", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Veritabanını sisteme kaydediyoruz
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();
app.UseCors("IzinVer");

// --- GÜVENLİK KATMAPI ---
app.Use(async (context, next) =>
{
    // Sadece ürün ekleme (POST), güncelleme (PUT) ve silme (DELETE) işlemlerinde şifre sor
    if (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "DELETE")
    {
        // Şifreyi aşağıdan değiştirebilirsin (Şu an: Adalyn2026!)
        if (!context.Request.Headers.TryGetValue("Admin-Sifresi", out var sifre) || sifre != "Adalyn2026!") 
        {
            context.Response.StatusCode = 401; // 401: Yetkisiz Giriş
            await context.Response.WriteAsync("Yetkisiz Islem! Sifre Yanlis.");
            return;
        }
    }
    await next();
});
// ------------------------

// SİHİRLİ KOD: Sistem çalıştığında veritabanı dosyası yoksa otomatik oluşturur
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 1. GET (Listele)
app.MapGet("/api/urunler", (AppDbContext db) => db.Urunler.ToList());

// 2. POST (Ekle)
app.MapPost("/api/urunler", (AppDbContext db, Urun yeniUrun) => {
    db.Urunler.Add(yeniUrun);
    db.SaveChanges(); // Kalıcı olarak dosyaya kaydet
    return Results.Ok(yeniUrun);
});

// 3. PUT (Güncelle)
app.MapPut("/api/urunler/{id}", (AppDbContext db, int id, Urun guncelUrun) => {
    var urun = db.Urunler.FirstOrDefault(u => u.Id == id);
    if (urun == null) return Results.NotFound();

    urun.Isim = guncelUrun.Isim;
    urun.Kategori = guncelUrun.Kategori;
    urun.KapakFoto = guncelUrun.KapakFoto;
    urun.DetayFotograflar = guncelUrun.DetayFotograflar;

    db.SaveChanges(); // Değişiklikleri kaydet
    return Results.Ok(urun);
});

// 4. DELETE (Sil)
app.MapDelete("/api/urunler/{id}", (AppDbContext db, int id) => {
    var urun = db.Urunler.FirstOrDefault(u => u.Id == id);
    if (urun == null) return Results.NotFound();

    db.Urunler.Remove(urun);
    db.SaveChanges(); // Silme işlemini kaydet
    return Results.Ok();
});

app.Run();