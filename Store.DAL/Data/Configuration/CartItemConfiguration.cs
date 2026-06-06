using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.DAL
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        /*------------------------------------------------------------------*/
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Quantity).IsRequired();
            builder.Property(c => c.AddedAt).IsRequired();

            builder.HasOne(c => c.Product)
                   .WithMany(p => p.CartItems)
                   .HasForeignKey(c => c.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
                   .WithMany()
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One user can have only one cart item per product
            builder.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();
        }
    }
}
