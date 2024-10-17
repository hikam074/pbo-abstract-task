using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAIN;
using ROBOTS;

namespace IKEMAMPUAN
{
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
