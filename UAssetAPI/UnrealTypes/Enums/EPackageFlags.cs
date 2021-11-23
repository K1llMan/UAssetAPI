using System;

namespace UAssetAPI.UnrealTypes.Enums
{
	/// <summary>
	/// Package flags, passed into UPackage::SetPackageFlags and related functions in the Unreal Engine
	/// </summary>
	[Flags]
	public enum EPackageFlags : uint
	{
		///<summary>No flags</summary>
		PKG_None = 0x00000000,
		///<summary>Newly created package, not saved yet. In editor only.</summary>
		PKG_NewlyCreated = 0x00000001,
		///<summary>Purely optional for clients.</summary>
		PKG_ClientOptional = 0x00000002,
		///<summary>Only needed on the server side.</summary>
		PKG_ServerSideOnly = 0x00000004,
		///<summary>This package is from "compiled in" classes.</summary>
		PKG_CompiledIn = 0x00000010,
		///<summary>This package was loaded just for the purposes of diffing</summary>
		PKG_ForDiffing = 0x00000020,
		///<summary>This is editor-only package (for example: editor module script package)</summary>
		PKG_EditorOnly = 0x00000040,
		///<summary>Developer module</summary>
		PKG_Developer = 0x00000080,
		///<summary>Loaded only in uncooked builds (i.e. runtime in editor)</summary>
		PKG_UncookedOnly = 0x00000100,
		///<summary>Package is cooked</summary>
		PKG_Cooked = 0x00000200,
		///<summary>Package doesn't contain any asset object (although asset tags can be present)</summary>
		PKG_ContainsNoAsset = 0x00000400,
		///<summary>Uses unversioned property serialization instead of versioned tagged property serialization</summary>
		PKG_UnversionedProperties = 0x00002000,
		///<summary>Contains map data (UObjects only referenced by a single ULevel) but is stored in a different package</summary>
		PKG_ContainsMapData = 0x00004000,
		///<summary>package is currently being compiled</summary>
		PKG_Compiling = 0x00010000,
		///<summary>Set if the package contains a ULevel/ UWorld object</summary>
		PKG_ContainsMap = 0x00020000,
		///<summary>???</summary>
		PKG_RequiresLocalizationGather = 0x00040000,
		///<summary>Set if the package was created for the purpose of PIE</summary>
		PKG_PlayInEditor = 0x00100000,
		///<summary>Package is allowed to contain UClass objects</summary>
		PKG_ContainsScript = 0x00200000,
		///<summary>Editor should not export asset in this package</summary>
		PKG_DisallowExport = 0x00400000,
		///<summary>This package should resolve dynamic imports from its export at runtime.</summary>
		PKG_DynamicImports = 0x10000000,
		///<summary>This package contains elements that are runtime generated, and may not follow standard loading order rules</summary>
		PKG_RuntimeGenerated = 0x20000000,
		///<summary>This package is reloading in the cooker, try to avoid getting data we will never need. We won't save this package.</summary>
		PKG_ReloadingForCooker = 0x40000000,
		///<summary>Package has editor-only data filtered out</summary>
		PKG_FilterEditorOnly = 0x80000000,
	}
}
