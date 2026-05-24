CREATE DATABASE IF NOT EXISTS arac_muayene_randevu CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;
USE arac_muayene_randevu;

CREATE TABLE IF NOT EXISTS arac_sahipleri (
    sahip_id INT AUTO_INCREMENT PRIMARY KEY,
    ad VARCHAR(50) NOT NULL,
    soyad VARCHAR(50) NOT NULL,
    tc_no VARCHAR(11) NOT NULL UNIQUE,
    telefon VARCHAR(20) NOT NULL,
    eposta VARCHAR(100) UNIQUE,
    adres VARCHAR(250) NOT NULL,
    kayit_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS araclar (
    arac_id INT AUTO_INCREMENT PRIMARY KEY,
    sahip_id INT NOT NULL,
    plaka VARCHAR(15) NOT NULL UNIQUE,
    marka VARCHAR(50) NOT NULL,
    model VARCHAR(50) NOT NULL,
    yil INT NOT NULL,
    yakit_turu VARCHAR(30) NOT NULL,
    arac_tipi VARCHAR(30) NOT NULL,
    FOREIGN KEY (sahip_id) REFERENCES arac_sahipleri(sahip_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CHECK (yil >= 1980)
);

CREATE TABLE IF NOT EXISTS istasyonlar (
    istasyon_id INT AUTO_INCREMENT PRIMARY KEY,
    istasyon_adi VARCHAR(100) NOT NULL,
    il VARCHAR(50) NOT NULL,
    ilce VARCHAR(50) NOT NULL,
    adres VARCHAR(250) NOT NULL,
    telefon VARCHAR(20) NOT NULL
);

CREATE TABLE IF NOT EXISTS personeller (
    personel_id INT AUTO_INCREMENT PRIMARY KEY,
    istasyon_id INT NOT NULL,
    ad VARCHAR(50) NOT NULL,
    soyad VARCHAR(50) NOT NULL,
    gorev VARCHAR(50) NOT NULL,
    telefon VARCHAR(20),
    FOREIGN KEY (istasyon_id) REFERENCES istasyonlar(istasyon_id)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS muayene_turleri (
    muayene_tur_id INT AUTO_INCREMENT PRIMARY KEY,
    tur_adi VARCHAR(80) NOT NULL UNIQUE,
    temel_ucret DECIMAL(10,2) NOT NULL,
    aciklama VARCHAR(250),
    CHECK (temel_ucret > 0)
);

CREATE TABLE IF NOT EXISTS randevular (
    randevu_id INT AUTO_INCREMENT PRIMARY KEY,
    arac_id INT NOT NULL,
    istasyon_id INT NOT NULL,
    muayene_tur_id INT NOT NULL,
    randevu_tarih DATE NOT NULL,
    randevu_saat TIME NOT NULL,
    durum VARCHAR(20) NOT NULL DEFAULT 'Bekliyor',
    aciklama VARCHAR(250),
    FOREIGN KEY (arac_id) REFERENCES araclar(arac_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (istasyon_id) REFERENCES istasyonlar(istasyon_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (muayene_tur_id) REFERENCES muayene_turleri(muayene_tur_id)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    UNIQUE (istasyon_id, randevu_tarih, randevu_saat),
    CHECK (durum IN ('Bekliyor','Tamamlandı','İptal'))
);

CREATE TABLE IF NOT EXISTS muayene_kayitlari (
    muayene_id INT AUTO_INCREMENT PRIMARY KEY,
    randevu_id INT NOT NULL UNIQUE,
    personel_id INT NOT NULL,
    sonuc VARCHAR(30) NOT NULL,
    kusur_aciklama VARCHAR(500),
    muayene_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (randevu_id) REFERENCES randevular(randevu_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (personel_id) REFERENCES personeller(personel_id)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    CHECK (sonuc IN ('Geçti','Kaldı','Tekrar Kontrol'))
);

CREATE TABLE IF NOT EXISTS odemeler (
    odeme_id INT AUTO_INCREMENT PRIMARY KEY,
    randevu_id INT NOT NULL,
    odeme_tarihi DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    tutar DECIMAL(10,2) NOT NULL,
    odeme_turu VARCHAR(30) NOT NULL,
    durum VARCHAR(20) NOT NULL DEFAULT 'Ödendi',
    FOREIGN KEY (randevu_id) REFERENCES randevular(randevu_id)
        ON DELETE CASCADE ON UPDATE CASCADE,
    CHECK (tutar > 0),
    CHECK (odeme_turu IN ('Nakit','Kredi Kartı','Havale')),
    CHECK (durum IN ('Ödendi','İade'))
);

DROP PROCEDURE IF EXISTS sp_AracSahibiEkle;
DROP PROCEDURE IF EXISTS sp_AracSahibiGuncelle;
DROP PROCEDURE IF EXISTS sp_AracSahibiSil;
DROP PROCEDURE IF EXISTS sp_AracSahibiListele;
DROP PROCEDURE IF EXISTS sp_AracEkle;
DROP PROCEDURE IF EXISTS sp_AracGuncelle;
DROP PROCEDURE IF EXISTS sp_AracSil;
DROP PROCEDURE IF EXISTS sp_AracListele;
DROP PROCEDURE IF EXISTS sp_IstasyonEkle;
DROP PROCEDURE IF EXISTS sp_IstasyonGuncelle;
DROP PROCEDURE IF EXISTS sp_IstasyonSil;
DROP PROCEDURE IF EXISTS sp_IstasyonListele;
DROP PROCEDURE IF EXISTS sp_PersonelEkle;
DROP PROCEDURE IF EXISTS sp_PersonelGuncelle;
DROP PROCEDURE IF EXISTS sp_PersonelSil;
DROP PROCEDURE IF EXISTS sp_PersonelListele;
DROP PROCEDURE IF EXISTS sp_MuayeneTuruEkle;
DROP PROCEDURE IF EXISTS sp_MuayeneTuruGuncelle;
DROP PROCEDURE IF EXISTS sp_MuayeneTuruSil;
DROP PROCEDURE IF EXISTS sp_MuayeneTuruListele;
DROP PROCEDURE IF EXISTS sp_RandevuEkle;
DROP PROCEDURE IF EXISTS sp_RandevuGuncelle;
DROP PROCEDURE IF EXISTS sp_RandevuSil;
DROP PROCEDURE IF EXISTS sp_RandevuListele;
DROP PROCEDURE IF EXISTS sp_MuayeneKaydiEkle;
DROP PROCEDURE IF EXISTS sp_MuayeneKaydiGuncelle;
DROP PROCEDURE IF EXISTS sp_MuayeneKaydiSil;
DROP PROCEDURE IF EXISTS sp_MuayeneKaydiListele;
DROP PROCEDURE IF EXISTS sp_OdemeEkle;
DROP PROCEDURE IF EXISTS sp_OdemeGuncelle;
DROP PROCEDURE IF EXISTS sp_OdemeSil;
DROP PROCEDURE IF EXISTS sp_OdemeListele;

DELIMITER $$

CREATE PROCEDURE sp_AracSahibiEkle(IN p_ad VARCHAR(50), IN p_soyad VARCHAR(50), IN p_tc_no VARCHAR(11), IN p_telefon VARCHAR(20), IN p_eposta VARCHAR(100), IN p_adres VARCHAR(250))
BEGIN
    INSERT INTO arac_sahipleri(ad, soyad, tc_no, telefon, eposta, adres)
    VALUES(p_ad, p_soyad, p_tc_no, p_telefon, p_eposta, p_adres);
END $$

CREATE PROCEDURE sp_AracSahibiGuncelle(IN p_sahip_id INT, IN p_ad VARCHAR(50), IN p_soyad VARCHAR(50), IN p_telefon VARCHAR(20), IN p_eposta VARCHAR(100), IN p_adres VARCHAR(250))
BEGIN
    UPDATE arac_sahipleri SET ad=p_ad, soyad=p_soyad, telefon=p_telefon, eposta=p_eposta, adres=p_adres WHERE sahip_id=p_sahip_id;
END $$

CREATE PROCEDURE sp_AracSahibiSil(IN p_sahip_id INT)
BEGIN
    DELETE FROM arac_sahipleri WHERE sahip_id=p_sahip_id;
END $$

CREATE PROCEDURE sp_AracSahibiListele()
BEGIN
    SELECT * FROM arac_sahipleri ORDER BY sahip_id DESC;
END $$

CREATE PROCEDURE sp_AracEkle(IN p_sahip_id INT, IN p_plaka VARCHAR(15), IN p_marka VARCHAR(50), IN p_model VARCHAR(50), IN p_yil INT, IN p_yakit_turu VARCHAR(30), IN p_arac_tipi VARCHAR(30))
BEGIN
    INSERT INTO araclar(sahip_id, plaka, marka, model, yil, yakit_turu, arac_tipi)
    VALUES(p_sahip_id, p_plaka, p_marka, p_model, p_yil, p_yakit_turu, p_arac_tipi);
END $$

CREATE PROCEDURE sp_AracGuncelle(IN p_arac_id INT, IN p_plaka VARCHAR(15), IN p_marka VARCHAR(50), IN p_model VARCHAR(50), IN p_yil INT, IN p_yakit_turu VARCHAR(30), IN p_arac_tipi VARCHAR(30))
BEGIN
    UPDATE araclar SET plaka=p_plaka, marka=p_marka, model=p_model, yil=p_yil, yakit_turu=p_yakit_turu, arac_tipi=p_arac_tipi WHERE arac_id=p_arac_id;
END $$

CREATE PROCEDURE sp_AracSil(IN p_arac_id INT)
BEGIN
    DELETE FROM araclar WHERE arac_id=p_arac_id;
END $$

CREATE PROCEDURE sp_AracListele()
BEGIN
    SELECT a.arac_id, a.plaka, a.marka, a.model, a.yil, a.yakit_turu, a.arac_tipi, CONCAT(s.ad,' ',s.soyad) AS arac_sahibi
    FROM araclar a INNER JOIN arac_sahipleri s ON a.sahip_id=s.sahip_id
    ORDER BY a.arac_id DESC;
END $$

CREATE PROCEDURE sp_IstasyonEkle(IN p_istasyon_adi VARCHAR(100), IN p_il VARCHAR(50), IN p_ilce VARCHAR(50), IN p_adres VARCHAR(250), IN p_telefon VARCHAR(20))
BEGIN
    INSERT INTO istasyonlar(istasyon_adi, il, ilce, adres, telefon)
    VALUES(p_istasyon_adi, p_il, p_ilce, p_adres, p_telefon);
END $$

CREATE PROCEDURE sp_IstasyonGuncelle(IN p_istasyon_id INT, IN p_istasyon_adi VARCHAR(100), IN p_il VARCHAR(50), IN p_ilce VARCHAR(50), IN p_adres VARCHAR(250), IN p_telefon VARCHAR(20))
BEGIN
    UPDATE istasyonlar SET istasyon_adi=p_istasyon_adi, il=p_il, ilce=p_ilce, adres=p_adres, telefon=p_telefon WHERE istasyon_id=p_istasyon_id;
END $$

CREATE PROCEDURE sp_IstasyonSil(IN p_istasyon_id INT)
BEGIN
    DELETE FROM istasyonlar WHERE istasyon_id=p_istasyon_id;
END $$

CREATE PROCEDURE sp_IstasyonListele()
BEGIN
    SELECT * FROM istasyonlar ORDER BY il, ilce;
END $$

CREATE PROCEDURE sp_PersonelEkle(IN p_istasyon_id INT, IN p_ad VARCHAR(50), IN p_soyad VARCHAR(50), IN p_gorev VARCHAR(50), IN p_telefon VARCHAR(20))
BEGIN
    INSERT INTO personeller(istasyon_id, ad, soyad, gorev, telefon)
    VALUES(p_istasyon_id, p_ad, p_soyad, p_gorev, p_telefon);
END $$

CREATE PROCEDURE sp_PersonelGuncelle(IN p_personel_id INT, IN p_istasyon_id INT, IN p_ad VARCHAR(50), IN p_soyad VARCHAR(50), IN p_gorev VARCHAR(50), IN p_telefon VARCHAR(20))
BEGIN
    UPDATE personeller SET istasyon_id=p_istasyon_id, ad=p_ad, soyad=p_soyad, gorev=p_gorev, telefon=p_telefon WHERE personel_id=p_personel_id;
END $$

CREATE PROCEDURE sp_PersonelSil(IN p_personel_id INT)
BEGIN
    DELETE FROM personeller WHERE personel_id=p_personel_id;
END $$

CREATE PROCEDURE sp_PersonelListele()
BEGIN
    SELECT p.personel_id, p.ad, p.soyad, p.gorev, p.telefon, i.istasyon_adi
    FROM personeller p INNER JOIN istasyonlar i ON p.istasyon_id=i.istasyon_id
    ORDER BY p.personel_id DESC;
END $$

CREATE PROCEDURE sp_MuayeneTuruEkle(IN p_tur_adi VARCHAR(80), IN p_temel_ucret DECIMAL(10,2), IN p_aciklama VARCHAR(250))
BEGIN
    INSERT INTO muayene_turleri(tur_adi, temel_ucret, aciklama)
    VALUES(p_tur_adi, p_temel_ucret, p_aciklama);
END $$

CREATE PROCEDURE sp_MuayeneTuruGuncelle(IN p_muayene_tur_id INT, IN p_tur_adi VARCHAR(80), IN p_temel_ucret DECIMAL(10,2), IN p_aciklama VARCHAR(250))
BEGIN
    UPDATE muayene_turleri SET tur_adi=p_tur_adi, temel_ucret=p_temel_ucret, aciklama=p_aciklama WHERE muayene_tur_id=p_muayene_tur_id;
END $$

CREATE PROCEDURE sp_MuayeneTuruSil(IN p_muayene_tur_id INT)
BEGIN
    DELETE FROM muayene_turleri WHERE muayene_tur_id=p_muayene_tur_id;
END $$

CREATE PROCEDURE sp_MuayeneTuruListele()
BEGIN
    SELECT * FROM muayene_turleri ORDER BY muayene_tur_id DESC;
END $$

CREATE PROCEDURE sp_RandevuEkle(IN p_arac_id INT, IN p_istasyon_id INT, IN p_muayene_tur_id INT, IN p_randevu_tarih DATE, IN p_randevu_saat TIME, IN p_aciklama VARCHAR(250))
BEGIN
    INSERT INTO randevular(arac_id, istasyon_id, muayene_tur_id, randevu_tarih, randevu_saat, aciklama)
    VALUES(p_arac_id, p_istasyon_id, p_muayene_tur_id, p_randevu_tarih, p_randevu_saat, p_aciklama);
END $$

CREATE PROCEDURE sp_RandevuGuncelle(IN p_randevu_id INT, IN p_arac_id INT, IN p_istasyon_id INT, IN p_muayene_tur_id INT, IN p_randevu_tarih DATE, IN p_randevu_saat TIME, IN p_durum VARCHAR(20), IN p_aciklama VARCHAR(250))
BEGIN
    UPDATE randevular SET arac_id=p_arac_id, istasyon_id=p_istasyon_id, muayene_tur_id=p_muayene_tur_id, randevu_tarih=p_randevu_tarih, randevu_saat=p_randevu_saat, durum=p_durum, aciklama=p_aciklama WHERE randevu_id=p_randevu_id;
END $$

CREATE PROCEDURE sp_RandevuSil(IN p_randevu_id INT)
BEGIN
    DELETE FROM randevular WHERE randevu_id=p_randevu_id;
END $$

CREATE PROCEDURE sp_RandevuListele()
BEGIN
    SELECT r.randevu_id, a.plaka, CONCAT(s.ad,' ',s.soyad) AS arac_sahibi, i.istasyon_adi, mt.tur_adi, r.randevu_tarih, r.randevu_saat, r.durum, r.aciklama
    FROM randevular r
    INNER JOIN araclar a ON r.arac_id=a.arac_id
    INNER JOIN arac_sahipleri s ON a.sahip_id=s.sahip_id
    INNER JOIN istasyonlar i ON r.istasyon_id=i.istasyon_id
    INNER JOIN muayene_turleri mt ON r.muayene_tur_id=mt.muayene_tur_id
    ORDER BY r.randevu_tarih, r.randevu_saat;
END $$

CREATE PROCEDURE sp_MuayeneKaydiEkle(IN p_randevu_id INT, IN p_personel_id INT, IN p_sonuc VARCHAR(30), IN p_kusur_aciklama VARCHAR(500))
BEGIN
    INSERT INTO muayene_kayitlari(randevu_id, personel_id, sonuc, kusur_aciklama)
    VALUES(p_randevu_id, p_personel_id, p_sonuc, p_kusur_aciklama);
END $$

CREATE PROCEDURE sp_MuayeneKaydiGuncelle(IN p_muayene_id INT, IN p_personel_id INT, IN p_sonuc VARCHAR(30), IN p_kusur_aciklama VARCHAR(500))
BEGIN
    UPDATE muayene_kayitlari SET personel_id=p_personel_id, sonuc=p_sonuc, kusur_aciklama=p_kusur_aciklama WHERE muayene_id=p_muayene_id;
END $$

CREATE PROCEDURE sp_MuayeneKaydiSil(IN p_muayene_id INT)
BEGIN
    DELETE FROM muayene_kayitlari WHERE muayene_id=p_muayene_id;
END $$

CREATE PROCEDURE sp_MuayeneKaydiListele()
BEGIN
    SELECT mk.muayene_id, r.randevu_id, a.plaka, CONCAT(p.ad,' ',p.soyad) AS personel_ad_soyad, mk.sonuc, mk.kusur_aciklama, mk.muayene_tarihi
    FROM muayene_kayitlari mk
    INNER JOIN randevular r ON mk.randevu_id=r.randevu_id
    INNER JOIN araclar a ON r.arac_id=a.arac_id
    INNER JOIN personeller p ON mk.personel_id=p.personel_id
    ORDER BY mk.muayene_id DESC;
END $$

CREATE PROCEDURE sp_OdemeEkle(IN p_randevu_id INT, IN p_tutar DECIMAL(10,2), IN p_odeme_turu VARCHAR(30))
BEGIN
    INSERT INTO odemeler(randevu_id, tutar, odeme_turu)
    VALUES(p_randevu_id, p_tutar, p_odeme_turu);
END $$

CREATE PROCEDURE sp_OdemeGuncelle(IN p_odeme_id INT, IN p_tutar DECIMAL(10,2), IN p_odeme_turu VARCHAR(30), IN p_durum VARCHAR(20))
BEGIN
    UPDATE odemeler SET tutar=p_tutar, odeme_turu=p_odeme_turu, durum=p_durum WHERE odeme_id=p_odeme_id;
END $$

CREATE PROCEDURE sp_OdemeSil(IN p_odeme_id INT)
BEGIN
    DELETE FROM odemeler WHERE odeme_id=p_odeme_id;
END $$

CREATE PROCEDURE sp_OdemeListele()
BEGIN
    SELECT o.odeme_id, r.randevu_id, a.plaka, CONCAT(s.ad,' ',s.soyad) AS arac_sahibi, o.odeme_tarihi, o.tutar, o.odeme_turu, o.durum
    FROM odemeler o
    INNER JOIN randevular r ON o.randevu_id=r.randevu_id
    INNER JOIN araclar a ON r.arac_id=a.arac_id
    INNER JOIN arac_sahipleri s ON a.sahip_id=s.sahip_id
    ORDER BY o.odeme_id DESC;
END $$

DROP TRIGGER IF EXISTS tg_RandevuSaatKontrol $$
CREATE TRIGGER tg_RandevuSaatKontrol
BEFORE INSERT ON randevular
FOR EACH ROW
BEGIN
    DECLARE kayit_sayisi INT;
    SELECT COUNT(*) INTO kayit_sayisi
    FROM randevular
    WHERE istasyon_id=NEW.istasyon_id
    AND randevu_tarih=NEW.randevu_tarih
    AND randevu_saat=NEW.randevu_saat
    AND durum <> 'İptal';

    IF kayit_sayisi > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT='Bu tarih ve saatte seçilen istasyonda randevu vardır.';
    END IF;
END $$

DROP TRIGGER IF EXISTS tg_MuayeneSonrasiRandevuTamamla $$
CREATE TRIGGER tg_MuayeneSonrasiRandevuTamamla
AFTER INSERT ON muayene_kayitlari
FOR EACH ROW
BEGIN
    UPDATE randevular
    SET durum='Tamamlandı'
    WHERE randevu_id=NEW.randevu_id;
END $$

DROP TRIGGER IF EXISTS tg_IptalRandevuOdemeKontrol $$
CREATE TRIGGER tg_IptalRandevuOdemeKontrol
BEFORE INSERT ON odemeler
FOR EACH ROW
BEGIN
    DECLARE randevu_durumu VARCHAR(20);
    SELECT durum INTO randevu_durumu
    FROM randevular
    WHERE randevu_id=NEW.randevu_id;

    IF randevu_durumu='İptal' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT='İptal edilmiş randevu için ödeme kaydı eklenemez.';
    END IF;
END $$

DELIMITER ;

INSERT IGNORE INTO muayene_turleri(tur_adi, temel_ucret, aciklama) VALUES
('Periyodik Muayene', 1800.00, 'Standart araç muayenesi'),
('Tekrar Muayene', 700.00, 'Kusurlu araçların tekrar kontrolü'),
('Tadilat Muayenesi', 1200.00, 'Araçta yapılan değişiklik sonrası kontrol'),
('Egzoz Kontrolü', 500.00, 'Egzoz emisyon kontrol işlemi');

CALL sp_AracSahibiEkle('Ahmet','Yılmaz','12345678901','05551234567','ahmet@mail.com','Bartın Merkez');
CALL sp_AracSahibiEkle('Ayşe','Demir','23456789012','05551112233','ayse@mail.com','Amasra');
CALL sp_AracSahibiEkle('Mehmet','Kaya','34567890123','05552223344','mehmet@mail.com','Ulus');
CALL sp_AracSahibiEkle('Zeynep','Çelik','45678901234','05553334455','zeynep@mail.com','Kurucaşile');

CALL sp_AracEkle(1,'74 AB 123','Renault','Clio',2018,'Benzin','Otomobil');
CALL sp_AracEkle(2,'74 AC 456','Fiat','Egea',2020,'Dizel','Otomobil');
CALL sp_AracEkle(3,'74 AD 789','Ford','Transit',2017,'Dizel','Ticari');
CALL sp_AracEkle(4,'74 AE 321','Toyota','Corolla',2021,'Benzin','Otomobil');

CALL sp_IstasyonEkle('Bartın Merkez Muayene','Bartın','Merkez','Sanayi Bölgesi','03781234567');
CALL sp_IstasyonEkle('Amasra Araç Muayene','Bartın','Amasra','Amasra sanayi yolu','03781230001');
CALL sp_IstasyonEkle('Ulus Araç Muayene','Bartın','Ulus','Ulus merkez sanayi','03781230002');

CALL sp_PersonelEkle(1,'Mehmet','Kaya','Muayene Görevlisi','05559876543');
CALL sp_PersonelEkle(1,'Hasan','Aydın','Danışma Personeli','05556667788');
CALL sp_PersonelEkle(2,'Merve','Yıldız','Muayene Görevlisi','05558889900');
CALL sp_PersonelEkle(3,'Emre','Koç','Muayene Görevlisi','05557778899');

CALL sp_RandevuEkle(1,1,1,'2026-05-25','10:00:00','İlk randevu');
CALL sp_RandevuEkle(2,1,1,'2026-05-25','11:00:00','Periyodik muayene');
CALL sp_RandevuEkle(3,2,2,'2026-05-26','12:00:00','Tekrar kontrol randevusu');
CALL sp_RandevuEkle(4,3,3,'2026-05-27','13:30:00','Tadilat sonrası muayene');

CALL sp_MuayeneKaydiEkle(1,1,'Geçti','Araçta ciddi kusur bulunmadı.');
CALL sp_MuayeneKaydiEkle(2,2,'Geçti','Genel kontrol sonucu uygundur.');
CALL sp_MuayeneKaydiEkle(3,3,'Tekrar Kontrol','Fren sistemi tekrar kontrol edilmelidir.');

CALL sp_OdemeEkle(1,1800.00,'Nakit');
CALL sp_OdemeEkle(2,1800.00,'Kredi Kartı');
CALL sp_OdemeEkle(3,700.00,'Havale');
CALL sp_OdemeEkle(4,1200.00,'Nakit');
