using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MAIN;
using IKEMAMPUAN;


namespace ROBOTS
{
    abstract class Robot
    {
        // atribut
        public string nama;
        public int energi;
        public int armor;
        public int serangan;

        public List<AbilityAktif> kemampuanAktif;

        // constructor
        public Robot(string _nama, int _energi, int _armor, int _serangan)
        {
            this.nama = _nama;
            this.energi = _energi;
            this.armor = _armor;
            this.serangan = _serangan;

            this.kemampuanAktif = new List<AbilityAktif>();
        }

        // method
        public abstract void serang(Robot target);
        public abstract void gunakanKemampuan(IKemampuan kemampuan);
        public abstract void cetakInformasi();
        public abstract void diserang(int serangan);

        public void gunakanKemampuanRandom(List<IKemampuan> listKemampuan)
        {
            Random random = new Random();
            int nomer = random.Next(listKemampuan.Count);
            IKemampuan kemampuanTerpilih = listKemampuan[nomer];
            // agar ada cooldown
            if (kemampuanTerpilih.timerCooldown == 0)
            {
                gunakanKemampuan(kemampuanTerpilih);
            }
        }
        public void kurangiCooldown(List<IKemampuan> listKemampuan)
        {
            foreach (IKemampuan kemampuan in listKemampuan)
            {
                if (kemampuan.timerCooldown > 0)
                {
                    kemampuan.timerCooldown--;
                }
            }
        }

        public void addAbilityAktif(SuperShield kemampuan, int durasi)
        {
            kemampuanAktif.Add(new AbilityAktif(kemampuan, durasi));
        }
        public void updateAbilityAktif()
        {
            for (int i = 0; i < kemampuanAktif.Count; i++)
            {
                kemampuanAktif[i].durasi--;
                if (kemampuanAktif[i].durasi <= 0)
                {
                    armor -= kemampuanAktif[i].kemampuan.shieldPoint;
                    Console.WriteLine($"[ABILITY END] {kemampuanAktif[i].kemampuan.namaKemampuan} {nama} berakhir! Armor {nama} : {armor}");
                    kemampuanAktif.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    class NormalRobot : Robot
    {
        public bool isAlive = false;

        // constructor
        public NormalRobot(string _nama, int _energi, int _armor, int _serangan) : base(_nama, _energi, _armor, _serangan)
        {
            this.isAlive = true;
        }

        // method derived
        public override void serang(Robot target)
        {
            target.diserang(this.serangan);
            Console.WriteLine($"[SERANG] {this.nama} menyerang {target.nama} sebesar {this.serangan}! Energi {target.nama} : {target.energi}");
        }
        public override void gunakanKemampuan(IKemampuan kemampuan)
        {
            kemampuan.Use(this);
        }
        public override void cetakInformasi()
        {
            Console.WriteLine($"[INFO] {this.nama} Energi: {this.energi}, Armor: {this.armor}, Serangan: {this.serangan}");
        }

        // method bawaan
        public override void diserang(int jmlSerangan)
        {
            int totalDamage = jmlSerangan - this.armor;
            if (totalDamage <= 0) { totalDamage = 0; }
            this.energi -= totalDamage;
            if (this.energi <= 0) { this.energi = 0; }
        }
        public void mati()
        {
            Console.WriteLine($"[DEFEATED] : Robot {this.nama} telah dikalahkan!");
            this.isAlive = false;
        }

    }
    class BosRobot : Robot
    {
        // atribut tambahan
        public int pertahanan;

        public bool isAlive = false;

        // constructor
        public BosRobot(string _nama, int _energi, int _armor, int _serangan, int _pertahanan) : base(_nama, _energi, _armor, _serangan)
        {
            this.pertahanan = _pertahanan;
            this.armor += this.pertahanan;
            this.isAlive = true;
        }

        // method derived
        public override void serang(Robot target)
        {
            target.diserang(this.serangan);
            Console.WriteLine($"[SERANG] : {this.nama} menyerang {target.nama} sebesar {this.serangan}! energi {target.nama} : {target.energi}");
        }
        public override void gunakanKemampuan(IKemampuan kemampuan)
        {
            kemampuan.Use(this);
        }
        public override void cetakInformasi()
        {
            Console.WriteLine($"[INFO] {this.nama} Energi: {this.energi}, Armor: {this.armor}, Serangan: {this.serangan}");
        }

        // method native
        public override void diserang(int jmlSerangan)
        {
            int totalDamage = jmlSerangan - this.armor;
            if (totalDamage <= 0) { totalDamage = 0; }
            this.energi -= totalDamage;
            if (this.energi <= 0) { this.energi = 0; }
        }
        public void mati()
        {
            Console.WriteLine($"[DEFEATED] : Boss {this.nama} telah dikalahkan!");
            this.isAlive = false;
        }
    }

    // class baru dengan polimorfism
    class Transformer : NormalRobot
    {
        public Random random;
        public Transformer(string _nama, int _energi, int _armor, int _serangan) : base(_nama, _energi, _armor, _serangan)
        {
            this.isAlive = true;
            random = new Random();
        }
        // ada chance untuk critical damage
        public override void serang(Robot target)
        {
            bool isCrit = random.Next(0, 100) < 25;
            int critDamage = this.serangan * 2;
            if (isCrit)
            {
                target.diserang(critDamage);
                Console.WriteLine($"[SERANG CRITICAL!] {this.nama} menyerang {target.nama} sebesar {critDamage}! Energi {target.nama} : {target.energi}");
            }
            else
            {
                target.diserang(this.serangan);
                Console.WriteLine($"[SERANG] {this.nama} menyerang {target.nama} sebesar {this.serangan}! Energi {target.nama} : {target.energi}");
            }
        }
    }
}