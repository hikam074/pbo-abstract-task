using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TUGAS2.Classes
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



    interface IKemampuan
    {
        public string namaKemampuan { get; }
        public void Use(Robot target);
        // cooldown
        public int cooldown { get; }
        public int timerCooldown { get; set; }
    }
    class Repair : IKemampuan
    {
        public string namaKemampuan { get; } = "Repair";
        public int repairPoint;
        public Repair(int jmlRepair)
        {
            this.repairPoint = jmlRepair;
        }
        public void Use(Robot target)
        {
            if (timerCooldown == 0)
            {
                target.energi += repairPoint;
                timerCooldown = cooldown; // set timer menjadi sebanyak cooldown
                Console.WriteLine($"[ABILITY-REPAIR] menambah energi {target.nama} sebesar {repairPoint}");
            }
        }
        // cooldown
        public int cooldown { get; } = 3; // setelah digunakan maka tidak bisa lagi digunakan pada 3 turn kedepan
        public int timerCooldown { get; set; } = 4; // berarti baru bisa dipakai di round 4
    }
    class ElectricShock : IKemampuan
    {
        public string namaKemampuan { get; } = "ElectricShock";
        public int electricDamage;
        public ElectricShock(int jmlSerangan)
        {
            this.electricDamage = jmlSerangan;
        }
        public void Use(Robot target)
        {
            if (timerCooldown == 0)
            {
                target.diserang(electricDamage);
                timerCooldown = cooldown; // set timer menjadi sebanyak cooldown
                Console.WriteLine($"[ABILITY-ELECTRIC SHOCK] menyerang {target.nama} sebesar {this.electricDamage}! Energi {target.nama} : {target.energi}");
            }
        }
        // cooldown
        public int cooldown { get; } = 4; // setelah digunakan maka tidak bisa lagi digunakan pada 4 turn kedepan
        public int timerCooldown { get; set; } = 2; // berarti baru bisa dipakai di round 2
    }
    class PlasmaCannon : IKemampuan
    {
        public string namaKemampuan { get; } = "PlasmaCannon";
        public int plasmaDamage;
        public PlasmaCannon(int jmlSerangan)
        {
            this.plasmaDamage = jmlSerangan;
        }
        public void Use(Robot target)
        {
            if (timerCooldown == 0)
            {
                target.diserang(plasmaDamage);
                timerCooldown = cooldown; // set timer menjadi sebanyak cooldown
                Console.WriteLine($"[ABILITY-PLASMA CANNON] : menyerang {target.nama} sebesar {this.plasmaDamage}! Energi {target.nama} : {target.energi}");
            }
        }
        // cooldown
        public int cooldown { get; } = 5; // setelah digunakan maka tidak bisa lagi digunakan pada 5 turn kedepan
        public int timerCooldown { get; set; } = 3; // berarti baru bisa dipakai di round 3
    }
    class SuperShield : IKemampuan
    {
        public string namaKemampuan { get; } = "SuperShield";
        public int shieldPoint;
        public int durasiShield;
        public SuperShield(int jmlShieldPoint, int durasiShield)
        {
            this.shieldPoint = jmlShieldPoint;
            this.durasiShield = durasiShield;
        }
        public void Use(Robot target)
        {
            if (timerCooldown == 0)
            {
                target.armor += shieldPoint;
                target.addAbilityAktif(this, durasiShield);
                timerCooldown = cooldown; // set timer menjadi sebanyak cooldown
                Console.WriteLine($"[ABILITY-SUPER SHIELD] : meningkatkan armor {target.nama} sebesar {this.shieldPoint}! Armor {target.nama} : {target.armor}");
            }
        }
        // cooldown
        public int cooldown { get; } = 4; // setelah digunakan maka tidak bisa lagi digunakan pada 4 turn kedepan
        public int timerCooldown { get; set; } = 2; // berarti baru bisa dipakai di round 2
    }
    class AbilityAktif
    {
        public SuperShield kemampuan;
        public int durasi;
        public AbilityAktif(SuperShield _kemampuan, int _durasi)
        {
            this.kemampuan = _kemampuan;
            this.durasi = _durasi;
        }
    }
}