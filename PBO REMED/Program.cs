using System;
using System.Collections.Generic;

public class Sekolah
{
    public string idSekolah { get; set; }
    public string namaSekolah { get; set; }
    public int jumlahPaketDiterima { get; set; }

    public Sekolah(string idSekolah, string namaSekolah, int jumlahPaketDiterima)
    {
        this.idSekolah = idSekolah;
        this.namaSekolah = namaSekolah;
        this.jumlahPaketDiterima = jumlahPaketDiterima;
    }

    public void TampilInfo()
    {
        Console.WriteLine($"ID Sekolah              : {idSekolah}");
        Console.WriteLine($"Nama Sekolah            : {namaSekolah}");
        Console.WriteLine($"Jumlah Paket Diterima   : {jumlahPaketDiterima}");
        Console.WriteLine();
    }
}

public abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }

    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }

    public int StokDapur
    {
        get { return stokDapur; }
        private set
        {
            if (value >= 0)
            {
                stokDapur = value;
            }
            else
            {
                Console.WriteLine("Error: Nilai stok dapur tidak boleh negatif!");
            }
        }
    }

    public PaketMBG(string idPaket, string namaPaket, int stokDapur)
    {
        this.idPaket = idPaket;
        this.namaPaket = namaPaket;
        this.StokDapur = stokDapur;
    }

    public void KurangiStok(int jumlah)
    {
        if (jumlah > 0 && jumlah <= StokDapur)
        {
            StokDapur -= jumlah;
        }
        else
        {
            Console.WriteLine("Error: Jumlah pengurangan tidak valid atau melebihi stok!");
        }
    }

    public abstract double HitungBiaya(int jumlah);
}

public class PaketReguler : PaketMBG
{
    public PaketReguler(string idPaket, string namaPaket, int stokDapur)
        : base(idPaket, namaPaket, stokDapur)
    {
    }

    public override double HitungBiaya(int jumlah)
    {
        return jumlah * 15000;
    }
}

public class PaketKhusus : PaketMBG
{
    public string jenisDiet { get; set; }

    public PaketKhusus(string idPaket, string namaPaket, string jenisDiet, int stokDapur)
        : base(idPaket, namaPaket, stokDapur)
    {
        this.jenisDiet = jenisDiet;
    }

    public override double HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
}

public interface IDapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiMBG : IDapatDidistribusikan
{
    public Sekolah SekolahTujuan { get; set; }
    public PaketMBG Paket { get; set; }
    public int Jumlah { get; set; }

    public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
    {
        SekolahTujuan = sekolah;
        Paket = paket;
        Jumlah = jumlah;
    }

    public void KirimPaket()
    {
        if (Jumlah <= 0)
        {
            Console.WriteLine("Distribusi GAGAL: Jumlah paket harus lebih dari 0.");
        }
        else if (Jumlah > Paket.StokDapur)
        {
            Console.WriteLine($"Distribusi GAGAL: Stok dapur tidak mencukupi. Sisa stok: {Paket.StokDapur}");
        }
        else
        {
            Paket.KurangiStok(Jumlah);
            SekolahTujuan.jumlahPaketDiterima += Jumlah;
            Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {Jumlah} paket makanan telah dikirim ke sekolah {SekolahTujuan.namaSekolah}.");
        }
    }
}

public class PengelolaMBG
{
    private List<Sekolah> daftarSekolah = new List<Sekolah>();
    private List<PaketMBG> daftarPaketMBG = new List<PaketMBG>();

    public void TambahSekolah(Sekolah sekolah)
    {
        daftarSekolah.Add(sekolah);
    }

    public void TambahPaket(PaketMBG paket)
    {
        daftarPaketMBG.Add(paket);
    }

    public void TampilkanDataMBG()
    {
        Console.WriteLine(" Informasi Data MBG ");
        foreach (Sekolah sekolah in daftarSekolah)
        {
            sekolah.TampilInfo();
        }

        Console.WriteLine(" Daftar Paket Makanan ");
        foreach (PaketMBG paket in daftarPaketMBG)
        {
            Console.WriteLine($"ID Paket: {paket.IdPaket} | Nama: {paket.NamaPaket} | Stok: {paket.StokDapur}");
        }
    }
}

public class Program
{
    static void Main()
    {
        Sekolah s1 = new Sekolah("SCH-01", "SMA Negeri Purwoharjo", 0);
        Sekolah s2 = new Sekolah("SCH-02", "SMA Negeri Cluring", 0);

        PaketMBG paket1 = new PaketReguler("PKT-01", "Paket Reguler", 500);
        PaketMBG paket2 = new PaketKhusus("PKT-02", "Paket Khusus", "Vegetarian", 300);

        Console.WriteLine(" PERHITUNGAN BIAYA DENGAN POLYMORPHISM ");
        Console.WriteLine($"Biaya Reguler 100 paket : Rp {paket1.HitungBiaya(100):N0}");
        Console.WriteLine($"Biaya Khusus 100 paket  : Rp {paket2.HitungBiaya(100):N0}");
        Console.WriteLine();

        Console.WriteLine(" PROSES DISTRIBUSI ");
        DistribusiMBG d1 = new DistribusiMBG(s1, paket1, 100);
        d1.KirimPaket();

        DistribusiMBG d2 = new DistribusiMBG(s2, paket2, 350);
        d2.KirimPaket();
        Console.WriteLine();

        Console.WriteLine(" SISA STOK ");
        Console.WriteLine($"Stok Paket Reguler : {paket1.StokDapur}");
        Console.WriteLine($"Stok Paket Khusus  : {paket2.StokDapur}");
        Console.WriteLine();

        Console.WriteLine(" SISTEM PENGELOLA ");
        PengelolaMBG pengelola = new PengelolaMBG();
        pengelola.TambahSekolah(s1);
        pengelola.TambahPaket(paket1);
        pengelola.TambahSekolah(s2);
        pengelola.TambahPaket(paket2);
        pengelola.TampilkanDataMBG();
    }
}