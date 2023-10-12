create database QuanlySV
go 

use QuanlySV
go

create table Lop (
	MaLop char(3) not null primary key,
	TenLop nvarchar(40) not null
)
go

create table Sinhvien(
	MaSV char(6) not null primary key,
	HotenSV nvarchar(40) not null,
	Ngaysinh datetime not null,
	MaLop char(3) not null,

	foreign key (MaLop) references Lop(MaLop)
)
go

insert into Lop(MaLop,TenLop)
values ('1',N'Công Nghệ Thông Tin')
insert into Lop(MaLop,TenLop)
values ('2',N'Quản Trị Kinh Doanh')
go

insert into Sinhvien(MaSV,HotenSV,Ngaysinh,MaLop)
values ('SV0001',N'Trần Văn Nam','1985-08-20','1')
insert into Sinhvien(MaSV,HotenSV,Ngaysinh,MaLop)
values ('SV0002',N'Bùi Hoàng Việt','2003-07-18','1')
insert into Sinhvien(MaSV,HotenSV,Ngaysinh,MaLop)
values ('SV0003',N'Nguyễn Thị Tuyết','1986-08-25','2')
insert into Sinhvien(MaSV,HotenSV,Ngaysinh,MaLop)
values ('SV0004',N'Nguyễn Kim Tuyến','1984-04-21','2')
go
