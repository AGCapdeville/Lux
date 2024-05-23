import math

map = {}

for x in range(5):
    for y in range(5):
        map[(x,y)] = {
            "prev":(0,0),
            "cost":1,
            'H':0,
            'G':0,
            'F':0
        }

player_position = {'x':0, 'y':0}
ending_position = {'x':0, 'y':4}

open_list = {}
closed_list = {}

def find_open_paths(current_position, end_position, map, current_open_list):
    temp_open_list = dict(current_open_list)
    cardinal_directions = [{'x':0, 'y':1}, {'x':1, 'y':0}, {'x':0, 'y':-1}, {'x':-1, 'y':0}]

    for direction in cardinal_directions:
        x = current_position['x'] + direction['x']
        y = current_position['y'] + direction['y']

        if ((x,y) in map):
            # New location cost + prev locations G (cost to get here)  
            G = map[(x,y)]["cost"] + map[(current_position['x'], current_position['y'])]["G"]
            H = math.sqrt((end_position['x'] - x)**2 + (end_position['y'] - y)**2)
            F = G + H

            # Is the position in the open list and if so is its F < than the currents positions F
            if ((x,y) in temp_open_list):
                if (F < temp_open_list((x,y))['F']):
                    temp_open_list[(x,y)]['G'] = G
                    temp_open_list[(x,y)]['H'] = H
                    temp_open_list[(x,y)]['F'] = F
                    temp_open_list[(x,y)]["prev"] = (x,y)
            else:
                temp_open_list[(x, y)] = {   # This line was added to add the position to temp_open_list
                    "prev": (current_position['x'], current_position['y']),
                    "cost": map[(x, y)]["cost"],
                    'H': H,
                    'G': G,
                    'F': F
                }
                    
    return temp_open_list

def find_best_path(current_open_list):
    temp_open_list = dict(current_open_list)
    smallest_f = 1000000
    smallest_key = (0,0)
    smallest_path = {}
    for key, value in temp_open_list.items():
        # print(value['F'], " < ", smallest_f)
        if (value['F'] < smallest_f):
            smallest_key = key
            smallest_f = value['F']
            smallest_path = value
    
    return smallest_key, smallest_path

def print_list(list_dictionary):
    for key, value in list_dictionary.items():
        print(key,':')
        for k, v, in value.items():
            print("    ", k,':', v)
    print('')
            

current_position = player_position
open_list = find_open_paths(current_position, ending_position, map, open_list)

# print("OPEN Before:")
# print_list(open_list)

key, path = find_best_path(open_list)

open_list.pop(key)
current_position = {
    'x': key[0],
    'y': key[1],
    "prev": path["prev"],
    "cost": path["cost"],
    'H': path['H'],
    'G': path['G'],
    'F': path['F']    
}

print("Current:")
print("    ", current_position, "\n")

closed_list[key] = path

# print("OPEN After:")
# print_list(open_list)
# print("\nCLOSED:")
# print_list(closed_list)

# while(current_position['x'] != ending_position['x'] and current_position['y'] != ending_position['y']):

    # Find all available paths from current position
    # open_list = find_open_paths(current_position, ending_position, map, open_list)

    # Find best H in open_list, remove it, and add it to closed list([iterable])
    # closed_list, open_list = find_best_path(open_list)

 
