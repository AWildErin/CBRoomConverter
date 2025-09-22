import os
import sys
import pathlib
import re

py_dir = pathlib.Path(__file__).parent.resolve()
template_file = f"{py_dir}/ClassTemplate.cs"
cs_output_path = os.path.abspath(f'{py_dir}/../CBRoomConverter/FunctionArguments')

func_regex = r'Function (.*)?\s*?\((.*)\)'
type_regex = r'(.*)\.(.*)'
default_val_regex = r'(.*?)\s*=\s*(.*)'
special_characters = ['#','$','%']

prop_template_text = ""

decls = [
    # CB Funcs
    'Function CreateDoor.Doors(lvl, x#, y#, z#, angle#, room.Rooms, dopen% = False,  big% = False, keycard% = False, code$="", useCollisionMesh% = False)',
    'Function LoadAnimMesh_Strict(File$,parent=0)',
    'Function LoadMesh_Strict(File$,parent=0)',
    'Function CreateItem.Items(name$, tempname$, x#, y#, z#, r%=0,g%=0,b%=0,a#=1.0,invSlots%=0)',
    'Function CreateSecurityCam.SecurityCams(x#, y#, z#, r.Rooms, screen% = False)',
    'Function CreateButton(x#,y#,z#, pitch#,yaw#,roll#=0)',
    'Function CreateWaypoint.WayPoints(x#,y#,z#,door.Doors, room.Rooms)',


    # BB Funcs
    'Function PositionEntity(entity#, x#, y#, z, global=0)',
    'Function CopyEntity(entity, parent=0)',
    'Function CreatePivot(parent=0)'
]

def extract_name_from_type(name_with_type) -> str:
    if '.' in name_with_type:
        type_match = re.search(type_regex, name_with_type)
        return type_match.groups()[0]
    
    return name_with_type.strip()

def remove_special_characters(name) -> str:
    for char in special_characters:
        name = name.replace(char, "")

    return name.strip()
        

def create_funcargs_class(decl):
    decl_match = re.search(func_regex, decl)

    func_name = extract_name_from_type(decl_match.groups()[0])

    func_output_path = pathlib.Path(f'{cs_output_path}/{func_name}FuncArgs.cs')
    func_output_path.parent.mkdir(parents=True, exist_ok=True)

    arguments = []

    arg_list = decl_match.groups()[1].split(',')
    for arg in arg_list:
        trimmed_arg = arg.strip()

        default_val_match = re.search(default_val_regex, trimmed_arg)
        if default_val_match is not None:
            name = extract_name_from_type(default_val_match.groups()[0])
            name = remove_special_characters(name)
            value = default_val_match.groups()[1]
            
            arguments.append(
                {
                    'name': name,
                    'optional': True
                }
            )

            continue

        name = extract_name_from_type(trimmed_arg)
        name = remove_special_characters(name)
        # No default value
        arguments.append(
            {
                'name': name,
                'optional': False
            }
        ) 

    # Create the C# properties
    cs_props = []

    # Inefficent but I really dont fancy figuring out a better way for a simple script
    # it's not my finest work, but this is a one and done script
    prop_index = 0
    for x in arguments:
        optional = x['optional']
        name = x['name']

        text = []
        text.append(f'\t[BlitzFuncArgIndex( "{name}", {prop_index}, Optional: {"true" if optional else "false"} )]\n')

        text.append('\tpublic')

        if optional is False:
            text.append(' required')

        text.append(' string')

        if optional:
            text.append('?')

        text.append(f' {name} {{ get; set; }}')

        cs_props.append(''.join(text))
        prop_index = prop_index + 1

    template = pathlib.Path(template_file).read_text()
    template = template.replace('{{FUNC_NAME}}', func_name)
    template = template.replace('{{CLASS_NAME}}', f'{func_name}FuncArgs')

    template = template.replace('{{FUNCTIONS}}', '\n\n'.join(cs_props))


    func_output_path.write_text(template)




for x in decls:
    create_funcargs_class(x)

