namespace Adalyn.API
{
    public class Urun
    {
        public int Id { get; set; }
        public string Isim { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public string KapakFoto { get; set; } = string.Empty;
        
        // Detay fotoğrafları birden fazla olacağı için liste (List) şeklinde tutuyoruz
        public List<string> DetayFotograflar { get; set; } = new List<string>();
    }
}