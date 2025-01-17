﻿#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using AK.Wwise.TreeView;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AkWwiseTreeView : TreeViewControl
{
	public TreeViewItem LastDoubleClickedItem = null;

	GUIStyle	m_filterBoxStyle	= null;
	GUIStyle	m_filterBoxCancelButtonStyle = null;
	string		m_filterString		= string.Empty;  
	static MonoScript DragDropHelperMonoScript = null;

#if UNITY_2017_2_OR_NEWER
	private void SaveExpansionStatusBeforePlay(PlayModeStateChange playMode)
	{
		if (playMode == PlayModeStateChange.ExitingEditMode)
			SaveExpansionStatus();
	}
#else
	private void SaveExpansionStatusBeforePlay()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			SaveExpansionStatus();
	}
#endif

	public AkWwiseTreeView()
	{
#if UNITY_2017_2_OR_NEWER
		EditorApplication.playModeStateChanged += SaveExpansionStatusBeforePlay;
#else
		EditorApplication.playmodeStateChanged += SaveExpansionStatusBeforePlay;
#endif
	}
		
	public class AkTreeInfo
    {
        public int ID = 0;
		public byte[] Guid = new byte[16];
        public AkWwiseProjectData.WwiseObjectType ObjectType;

		public AkTreeInfo(int id, AkWwiseProjectData.WwiseObjectType objType)
		{
			ID = id;
			ObjectType = objType;
		}

        public AkTreeInfo(int id, byte[] guid, AkWwiseProjectData.WwiseObjectType objType)
        {
            ID = id;
            ObjectType = objType;
			Guid = guid;
        }
    }

    TreeViewItem AddPathToTreeItem(TreeViewItem item, AkWwiseProjectData.AkInformation AkInfo)
    {
        TreeViewItem parentItem = item;

		string path = "/" + RootItem.Header + "/" + item.Header;

        for (int i = 0; i < AkInfo.PathAndIcons.Count; i++ )
        {
            AkWwiseProjectData.PathElement PathElem = AkInfo.PathAndIcons[i];
            TreeViewItem childItem = parentItem.FindItemByName(PathElem.ElementName);

			path = path + "/" + PathElem.ElementName;

            if (childItem == null)
            {
                if (i != AkInfo.PathAndIcons.Count - 1)
                {
					childItem = parentItem.AddItem(PathElem.ElementName, new AkTreeInfo(0, Guid.Empty.ToByteArray(), PathElem.ObjectType), GetExpansionStatus(path));
				}
				else
                {
					bool isDraggable = !(PathElem.ObjectType == AkWwiseProjectData.WwiseObjectType.STATEGROUP || PathElem.ObjectType == AkWwiseProjectData.WwiseObjectType.SWITCHGROUP);
					childItem = parentItem.AddItem(PathElem.ElementName, isDraggable, GetExpansionStatus(path), new AkTreeInfo(AkInfo.ID, AkInfo.Guid, PathElem.ObjectType));
                }
            }

            AddHandlerEvents(childItem);
            parentItem = childItem;
        }

        return parentItem;
    }

    public void SetRootItem(string Header, AkWwiseProjectData.WwiseObjectType ObjType)
    {
        RootItem.Items.Clear();
        RootItem.Header = Header;
        RootItem.DataContext = new AkTreeInfo(0, ObjType);
        AddHandlerEvents(RootItem);

		RootItem.IsExpanded = GetExpansionStatus("/" + RootItem.Header);
    }

	public void PopulateItem(TreeViewItem attachTo, string itemName, List<AkWwiseProjectData.AkInfoWorkUnit> workUnits)
    {
		TreeViewItem attachPoint = attachTo.AddItem(itemName, false, GetExpansionStatus("/" + RootItem.Header + "/" + itemName), new AkTreeInfo(0, AkWwiseProjectData.WwiseObjectType.PHYSICALFOLDER));

		foreach (AkWwiseProjectData.AkInfoWorkUnit wwu in workUnits)
        {
			foreach(AkWwiseProjectData.AkInformation akInfo in wwu.List)
			{
				AddHandlerEvents(AddPathToTreeItem(attachPoint, akInfo));
			}
        }

        AddHandlerEvents(attachPoint);
    }

    public void PopulateItem(TreeViewItem attachTo, string itemName, List<AkWwiseProjectData.EventWorkUnit> Events)
    {
		List<AkWwiseProjectData.AkInfoWorkUnit> akInfoWwu = new List<AkWwiseProjectData.AkInfoWorkUnit> (Events.Count);
		for(int i = 0; i < Events.Count; i++)
		{
			akInfoWwu.Add(new AkWwiseProjectData.AkInfoWorkUnit());
			akInfoWwu[i].PhysicalPath = Events[i].PhysicalPath;
			akInfoWwu[i].ParentPhysicalPath = Events[i].ParentPhysicalPath;
			akInfoWwu[i].Guid = Events[i].Guid;
			akInfoWwu[i].List = Events[i].List.ConvertAll(x => (AkWwiseProjectData.AkInformation)x);
		}
        
		PopulateItem(attachTo, itemName, akInfoWwu);
    }


	public void PopulateItem(TreeViewItem attachTo, string itemName, List<AkWwiseProjectData.GroupValWorkUnit> GroupWorkUnits)
    {
		TreeViewItem attachPoint = attachTo.AddItem(itemName, false, GetExpansionStatus("/" + RootItem.Header + "/" + itemName), new AkTreeInfo(0, AkWwiseProjectData.WwiseObjectType.PHYSICALFOLDER));

		foreach (AkWwiseProjectData.GroupValWorkUnit wwu in GroupWorkUnits)
        {
			foreach(AkWwiseProjectData.GroupValue group in wwu.List)
			{
            	TreeViewItem groupItem = AddPathToTreeItem(attachPoint, group);
            	AddHandlerEvents(groupItem);
					
            	for (int i = 0; i < group.values.Count; i++)
            	{
					TreeViewItem item = groupItem.AddItem(group.values[i], true, false, new AkTreeInfo(group.valueIDs[i], group.ValueGuids[i].bytes, group.ValueIcons[i].ObjectType));
            	    AddHandlerEvents(item);
            	}
			}
        }

        AddHandlerEvents(attachPoint);
    }

    /// <summary>
    /// Handler functions for TreeViewControl
    /// </summary>

    void AddHandlerEvents(TreeViewItem item)
    {
        // Uncomment this when we support right click
        item.Click = new System.EventHandler(HandleClick);
        item.Dragged = new System.EventHandler(PrepareDragDrop);
        item.CustomIconBuilder = new System.EventHandler(CustomIconHandler);
    }

    void HandleClick(object sender, System.EventArgs args)
    {
		if(Event.current.button == 0)
		{
			if((args as TreeViewItem.ClickEventArgs).m_clickCount == 2)
			{
				LastDoubleClickedItem = (TreeViewItem)sender;
				
				if(LastDoubleClickedItem.HasChildItems())
				{
					LastDoubleClickedItem.IsExpanded = !LastDoubleClickedItem.IsExpanded;
				}
			} 
		}
    }
	
    void PrepareDragDrop(object sender, System.EventArgs args)
    {
        TreeViewItem item = (TreeViewItem)sender;
        try
        {
            if (item == null || !item.IsDraggable)
            {
                return;
            }

			UnityEngine.Object[] objectReferences = new UnityEngine.Object[1];
			AkTreeInfo treeInfo = (AkTreeInfo)item.DataContext;

			AkDragDropData DDData = null;

			string objType = GetObjectType(treeInfo.ObjectType);
			if (objType == "State" || objType == "Switch")
            {
				AkDragDropGroupData DDGroupData = new AkDragDropGroupData();
				AkTreeInfo ParentTreeInfo = (AkTreeInfo)item.Parent.DataContext;
				DDGroupData.groupGuid = new Guid( ParentTreeInfo.Guid);
				DDGroupData.groupID = ParentTreeInfo.ID;
				DDData = DDGroupData;
			}
			else
            {
				DDData = new AkDragDropData();
            }

			DDData.name = item.Header;
			DDData.guid = new Guid(treeInfo.Guid);
			DDData.ID = treeInfo.ID;
			DDData.typeName = objType;

			objectReferences[0] = DragDropHelperMonoScript;
			GUIUtility.hotControl = 0;
			DragAndDrop.objectReferences = objectReferences;
			DragAndDrop.SetGenericData(AkDragDropHelper.DragDropIdentifier, DDData);
			DragAndDrop.StartDrag("Dragging an AkObject");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    string GetObjectType(AkWwiseProjectData.WwiseObjectType item)
    {
        string type = "undefined";
        switch (item)
        {
            case AkWwiseProjectData.WwiseObjectType.AUXBUS:
                type = "AuxBus";
                break;
            case AkWwiseProjectData.WwiseObjectType.EVENT:
                type = "Event";
                break;
            case AkWwiseProjectData.WwiseObjectType.SOUNDBANK:
                type = "Bank";
                break;
            case AkWwiseProjectData.WwiseObjectType.STATE:
                type = "State";
                break;
            case AkWwiseProjectData.WwiseObjectType.SWITCH:
                type = "Switch";
                break;
            case AkWwiseProjectData.WwiseObjectType.GAMEPARAMETER:
                type = "GameParameter";
                break;
        }

        return type;
    }

	private void ShowButtonTextureInternal(Texture2D texture)
	{
		if (null == texture || m_forceButtonText)
		{
			GUILayout.Button("", GUILayout.MaxWidth(16));
		}
		else
		{
			ShowButtonTexture(texture);
		}
	}

    public void CustomIconHandler(object sender, System.EventArgs args)
    {
        TreeViewItem item = (TreeViewItem)sender;
        AkTreeInfo treeInfo = (AkTreeInfo)item.DataContext;
        switch (treeInfo.ObjectType)
        {
            case AkWwiseProjectData.WwiseObjectType.AUXBUS:
				ShowButtonTextureInternal(m_textureWwiseAuxBusIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.BUS:
				ShowButtonTextureInternal(m_textureWwiseBusIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.EVENT:
            case AkWwiseProjectData.WwiseObjectType.GAMEPARAMETER:
				ShowButtonTextureInternal(m_textureWwiseEventIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.FOLDER:
				ShowButtonTextureInternal(m_textureWwiseFolderIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.PHYSICALFOLDER:
				ShowButtonTextureInternal(m_textureWwisePhysicalFolderIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.PROJECT:
				ShowButtonTextureInternal(m_textureWwiseProjectIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.SOUNDBANK:
				ShowButtonTextureInternal(m_textureWwiseSoundbankIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.STATE:
				ShowButtonTextureInternal(m_textureWwiseStateIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.STATEGROUP:
				ShowButtonTextureInternal(m_textureWwiseStateGroupIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.SWITCH:
				ShowButtonTextureInternal(m_textureWwiseSwitchIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.SWITCHGROUP:
				ShowButtonTextureInternal(m_textureWwiseSwitchGroupIcon);
                break;
            case AkWwiseProjectData.WwiseObjectType.WORKUNIT:
				ShowButtonTextureInternal(m_textureWwiseWorkUnitIcon);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Wwise logos
    /// </summary>
    Texture2D m_textureWwiseAuxBusIcon = null;
    Texture2D m_textureWwiseBusIcon = null;
    Texture2D m_textureWwiseEventIcon = null;
    Texture2D m_textureWwiseFolderIcon = null;
    Texture2D m_textureWwisePhysicalFolderIcon = null;
    Texture2D m_textureWwiseProjectIcon = null;
    Texture2D m_textureWwiseSoundbankIcon = null;
    Texture2D m_textureWwiseStateIcon = null;
    Texture2D m_textureWwiseStateGroupIcon = null;
    Texture2D m_textureWwiseSwitchIcon = null;
    Texture2D m_textureWwiseSwitchGroupIcon = null;
    Texture2D m_textureWwiseWorkUnitIcon = null;

    /// <summary>
    /// TreeViewControl overrides for our custom logos
    /// </summary>

    public override void AssignDefaults()
    {
        base.AssignDefaults();
        string tempWwisePath = "Assets/Wwise/Editor/WwiseWindows/TreeViewControl/";
        m_textureWwiseAuxBusIcon = GetTexture(tempWwisePath + "auxbus_nor.png");
        m_textureWwiseBusIcon = GetTexture(tempWwisePath + "bus_nor.png");
        m_textureWwiseEventIcon = GetTexture(tempWwisePath + "event_nor.png");
        m_textureWwiseFolderIcon = GetTexture(tempWwisePath + "folder_nor.png");
        m_textureWwisePhysicalFolderIcon = GetTexture(tempWwisePath + "physical_folder_nor.png");
        m_textureWwiseProjectIcon = GetTexture(tempWwisePath + "wproj.png");
        m_textureWwiseSoundbankIcon = GetTexture(tempWwisePath + "soundbank_nor.png");
        m_textureWwiseStateIcon = GetTexture(tempWwisePath + "state_nor.png");
        m_textureWwiseStateGroupIcon = GetTexture(tempWwisePath + "stategroup_nor.png");
        m_textureWwiseSwitchIcon = GetTexture(tempWwisePath + "switch_nor.png");
        m_textureWwiseSwitchGroupIcon = GetTexture(tempWwisePath + "switchgroup_nor.png");
        m_textureWwiseWorkUnitIcon = GetTexture(tempWwisePath + "workunit_nor.png");

		if(m_filterBoxStyle == null)
		{
			GUISkin InspectorSkin = ScriptableObject.Instantiate(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)) as GUISkin;
			InspectorSkin.hideFlags = HideFlags.HideAndDontSave;
			m_filterBoxStyle = InspectorSkin.FindStyle ("SearchTextField");
			m_filterBoxCancelButtonStyle = InspectorSkin.FindStyle ("SearchCancelButton");
		}

		if(DragDropHelperMonoScript == null)
		{
			MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
			for(int i = 0; i < scripts.Length; i++)
			{
				if(scripts[i].GetClass() == typeof(AkDragDropHelper))
				{
					DragDropHelperMonoScript = scripts[i];
					break;
				}
			}
		}
    }

    public override void DisplayTreeView(TreeViewControl.DisplayTypes displayType)
    {
    	if( AkWwisePicker.WwiseProjectFound )
    	{
			string filterString = m_filterString;
	
			if(m_filterBoxStyle == null)
			{
				m_filterBoxStyle = (ScriptableObject.Instantiate(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)) as GUISkin).FindStyle ("SearchTextField");
				m_filterBoxCancelButtonStyle = (ScriptableObject.Instantiate (EditorGUIUtility.GetBuiltinSkin (EditorSkin.Inspector)) as GUISkin).FindStyle ("SearchCancelButton");
			}
			
			GUILayout.BeginHorizontal("Box");
			{
				m_filterString = GUILayout.TextField(m_filterString, m_filterBoxStyle);
				if(GUILayout.Button ("", m_filterBoxCancelButtonStyle))
				{
					m_filterString = "";
				};
			}
			GUILayout.EndHorizontal ();
	
			if(!m_filterString.Equals(filterString))
			{
				if(filterString.Equals(string.Empty))
				{
					SaveExpansionStatus();
				}
	
				FilterTreeview(RootItem);
	
				if(m_filterString.Equals(string.Empty))
				{
					string path = "";
					RestoreExpansionStatus(RootItem, ref path);
				}
			}
	
	        base.DisplayTreeView(displayType);
		}
		else
		{
			GUILayout.Label ("Wwise Project not found at path:");
			GUILayout.Label (AkUtilities.GetFullPath(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath));
			GUILayout.Label ("Wwise Picker will not be usable.");
		}
    }

	bool FilterTreeview(TreeViewItem in_item)
	{
		in_item.IsHidden = in_item.Header.IndexOf(m_filterString, StringComparison.OrdinalIgnoreCase) < 0;
		in_item.IsExpanded = true;

		for(int i = 0; i < in_item.Items.Count; i++)
		{
			if(!FilterTreeview(in_item.Items[i]))
				in_item.IsHidden = false;
		}

		return in_item.IsHidden;
	}

	void RestoreExpansionStatus(TreeViewItem in_item, ref string in_path)
	{
		in_path = in_path + "/" + in_item.Header;

		in_item.IsExpanded = GetExpansionStatus (in_path);
				
		for(int i = 0; i < in_item.Items.Count; i++)
		{
			RestoreExpansionStatus(in_item.Items[i], ref in_path);
		}
		
		in_path = in_path.Remove(in_path.LastIndexOf('/'));
	}

	public void SaveExpansionStatus()
	{
		if( AkWwisePicker.WwiseProjectFound == true )
		{
			if( RootItem.Header == "Root item" )
			{
				// We were unpopulated, no need to save. But we still need to display the correct data, though.
				AkWwisePicker.PopulateTreeview();
				return;
			}
		
			if( AkWwiseProjectInfo.GetData () != null )
			{
				AkWwiseProjectInfo.GetData ().ExpandedItems.Clear ();
	
				string path = string.Empty;
	
				if(RootItem.HasChildItems() && RootItem.IsExpanded)
					SaveExpansionStatus (RootItem, path);
	
				AkWwiseProjectInfo.GetData ().ExpandedItems.Sort ();
				EditorUtility.SetDirty(AkWwiseProjectInfo.GetData());
			}
		}
	}

	private void SaveExpansionStatus(TreeViewItem in_item, string in_path)
	{
		in_path = in_path + "/" + in_item.Header;

		AkWwiseProjectInfo.GetData().ExpandedItems.Add(in_path);

		for(int i = 0; i < in_item.Items.Count; i++)
		{
			if(in_item.Items[i].HasChildItems() && in_item.Items[i].IsExpanded)
				SaveExpansionStatus(in_item.Items[i], in_path);
		}

		in_path = in_path.Remove(in_path.LastIndexOf('/'));
	}

	public bool GetExpansionStatus(string in_path)
	{
		return AkWwiseProjectInfo.GetData ().ExpandedItems.BinarySearch (in_path) >= 0;
	}

	public void SetScrollViewPosition(Vector2 in_pos)
	{
		m_scrollView = in_pos;
	}

	public TreeViewItem GetItemByPath(string in_path)
	{
		string[] headers = in_path.Split('/');

		if(!RootItem.Header.Equals(headers[0]))
			return null;

		TreeViewItem item = RootItem;

		for (int i = 1; i < headers.Length; i++)
		{
			item = item.Items.Find(x => x.Header.Equals(headers[i]));

			if(item == null)
				return null;
		}

		return item;
	}

	public TreeViewItem GetItemByGuid(Guid in_guid)
	{
		return GetItemByGuid (RootItem, in_guid);
	}

	public TreeViewItem GetItemByGuid(TreeViewItem in_item, Guid in_guid)
	{
		Guid itemGuid = new Guid ((in_item.DataContext as AkWwiseTreeView.AkTreeInfo).Guid);

		if(itemGuid.Equals(in_guid))
			return in_item;
		
		for(int i = 0; i < in_item.Items.Count; i++)
		{
			TreeViewItem item = GetItemByGuid(in_item.Items[i], in_guid);
			
			if(item != null)
				return item;
		}
		
		return null;
	}

	public TreeViewItem GetSelectedItem()
	{		
		return GetSelectedItem (RootItem);
	}
	
	public TreeViewItem GetSelectedItem(TreeViewItem in_item)
	{
		if(in_item.IsSelected)
			return in_item;
		
		for(int i = 0; i < in_item.Items.Count; i++)
		{
			TreeViewItem item = GetSelectedItem(in_item.Items[i]);
			
			if(item != null)
				return item;
		}
		
		return null;
	}
}
#endif