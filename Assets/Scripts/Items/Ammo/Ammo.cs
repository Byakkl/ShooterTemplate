using UnityEngine;

public class Ammo : IStackable
{
    AmmoType ammoType = AmmoType.Default;

    public int damage;

    private int _quantity;
    public int quantity { get => _quantity; set => _quantity = value; }

    private int _maxStack;
    public int maxStack { get => _maxStack; set => _maxStack = value; }

    public Ammo( int a_quantity = 1, AmmoType a_type = AmmoType.Default){
        _maxStack = 10;
        ammoType = a_type;
        _quantity = a_quantity;
    }

    public Ammo(AmmoType type){
        ammoType = type;
    }

    public void DecreaseQuantity(int a_quantity)
    {
        _quantity -= a_quantity;
        if(_quantity < 0)
        _quantity = 0;
    }

    public int IncreaseQuantity(ref IStackable a_other){
        Ammo casted = a_other as Ammo;
        if(a_other == null || casted.ammoType != ammoType)
            return a_other.quantity;
        
        a_other.quantity = IncreaseQuantity(a_other.quantity);
        return a_other.quantity;
    }

    public int IncreaseQuantity(int a_quantity)
    {
        int remainder = 0;
        _quantity += a_quantity;
        if(_quantity > _maxStack){
            remainder = _quantity - _maxStack;
            _quantity = _maxStack;
        }

        return remainder;
    }
}