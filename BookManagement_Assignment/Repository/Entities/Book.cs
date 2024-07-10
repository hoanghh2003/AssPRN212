using System;
using System.Collections.Generic;

namespace Repository.Entities;

public partial class Book
{
    public int BookId { get; set; }

    public string BookName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public string Author { get; set; } = null!;
    //Trong thông tin sách có 1 chút manh mối về thể loại sách (Chỉ có mã chủ đè, mã thể loại, Còn muốn biết tên thể loại thì không được
    //bắ buộc bạn phải join SQL)
    public int BookCategoryId { get; set; }

    //Thông tin của cuốn sách thì có thông tin thể loại sách
    public virtual BookCategory BookCategory { get; set; } = null!;
}
