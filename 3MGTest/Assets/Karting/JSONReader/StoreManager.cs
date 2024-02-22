using System;
using System.Collections.Generic;

public class StoreManager : BaseConfig<StoreManager, Item>
{
	#region implemented abstract members of BaseConfig

	public override void Save()
	{
		throw new NotImplementedException();
	}

	public override string ResourceName
	{
		get
		{
			return "Store";
		}
	}

	public override string RootKey
	{
		get
		{
			return "Store";
		}
	}

	public override Func<IDictionary<string, object>, Item> ItemConverter
	{
		get
		{
			return obj => new Item(obj);
		}
	}

	protected override void PostLoad()
	{
		base.PostLoad();
		//Items.Sort (Util.GetComparisonAscending<ReadAlphabet, int> (w => w.Sequence));
	}
	#endregion

	public static Item GetItemById(string id)
	{
		return Util.FirstOrDefault(Instance.Items, c => c.Id == id);
	}	
	public static List<Item> AllItems
	{
		get
		{
			return Instance.Items;
		}
	}
}

public class Item : BaseModel
{
	public string Id
	{
		get
		{
			return GetString(Schema.Item.Id);
		}
	}

	public string Name
	{
		get
		{
			return GetString(Schema.Item.Name);
		}
	}

	public bool IsUnLock
	{
		get
		{
			string sKey = Id;
			if (!LocalStorage.MyPlayerPrefs.HasKey(sKey))
			{
				if (LocalStorage.KartColor.Equals(Color))
					LocalStorage.MyPlayerPrefs.SetBool(sKey, true);
				else
					LocalStorage.MyPlayerPrefs.SetBool(sKey, false);
			}
			return LocalStorage.MyPlayerPrefs.GetBool(sKey);
		}
		set
		{
			string sKey = Id;
			LocalStorage.MyPlayerPrefs.SetBool(sKey, value);
		}
	}

	public string Color
	{
		get
		{
			return GetString(Schema.Item.Color);
		}
	}

	public int CurrentPrice
	{
		get
		{
			if (LocalStorage.MyPlayerPrefs.HasKey("prive_" + Id))
				return LocalStorage.MyPlayerPrefs.GetInt("prive_" + Id);
			else
				return DefaultPrice;
		}
	}

	public int DefaultPrice
	{
		get
		{
			return GetInt(Schema.Item.Price);
		}
	}	
	public Item(IDictionary<string, object> doc) : base(doc)
	{
	}
}

namespace Schema
{
	public class Item
	{
		public const string Id = "id";
		public const string Name = "nm";
		public const string Color = "clr";
		public const string Price = "pr";
	}
}