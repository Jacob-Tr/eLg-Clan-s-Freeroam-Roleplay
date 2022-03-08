let list_selected = 0, dialog_name = null, dialogid = 0, type = -1;

function onListItemClick(item)
{
	if(list_selected != item)
	{
		document.getElementById("item" + list_selected).setAttribute("class", "");
		document.getElementById("item" + item).setAttribute("class", "selected");
		list_selected = item;
	}
}

function onButtonClick(button)
{
	if(dialog_name != null && dialog_name != "")
	{
		let inputtext_value = null;
		
		if(document.getElementById("text_input") != null || document.getElementById("password_input") != null) 
		{
			if(document.getElementById("password_input") != null) inputtext_value = document.getElementById("password_input").value;
			else inputtext_value = document.getElementById("text_input").value;
			mp.trigger("DialogResponse", dialogid, dialog_name, button, inputtext_value);
		}
		else mp.trigger("DialogResponse", dialogid, dialog_name, button, list_selected);
	}
}

function createDialog(id, name, caption, info, buttons, list_items, text_input, password_input)
{
	if(name != "")
	{
		dialog_name = name;
		list_selected = 0;
		
		dialogid = id;

		let caption_str = "",
			info_str = "",
			list_items_str = "",
			inputs_str = "",
			buttons_str = "";

		if(caption != "") caption_str = "<div id='caption'>" + caption + "</div>";
		if(info != "") info_str = "<div id='body'>" + info + "</div>";
		if(list_items.length > 0)
		{
			document.getElementById("dialog").setAttribute('style', 'height: 43vh;width: 20vw;size: absolute;overflow_x: none;overflow_y: auto;');
			list_items_str = "<div id='list'><ul>";
			for(i = 0; i < list_items.length; i++)
			{ 
				if(i == list_selected) list_items_str += "<li id='item" + i + "' class='selected' onclick='onListItemClick(" + i + ")'>" + list_items[i] + "</li>";
				else list_items_str += "<li id='item" + i + "' onclick='onListItemClick(" + i + ")'>" + list_items[i] + "</li>";
			}
			list_items_str += "</ul></div>";
		}
		if(text_input != "" || password_input)
		{
			inputs_str = "<div id='input'>";
			if(text_input != "")
			{ 
				inputs_str += "<input id='text_input' type='text' placeholder='" + text_input + "'/>";
			}
			if(password_input != "")
			{ 
				inputs_str += "<input id='password_input' type='password' placeholder='" + password_input + "'/>";
			}
			inputs_str += "</div>";
		}
		if(buttons.length > 0)
		{
			buttons_str = "<div id='buttons'>";
			for(i = 0; i < buttons.length; i++)
			{ 
				buttons_str += "<button id='" + i + "' type='button' onclick='onButtonClick(" + i + ")'>" + buttons[i] + "</button>";
			}
			buttons_str += "</div>";
		}

		//ok
		document.getElementById("dialog").innerHTML = caption_str + info_str + list_items_str + inputs_str + buttons_str;
	}
}