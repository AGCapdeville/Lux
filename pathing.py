import math

space_enum = {
    "empty": 0,
    "block": 1,
    "player": 2,
    "enemy": 3,
    # Add more mappings as needed
}



class Node:
    def __init__(self, x, y):
        self.position = {'x': x, 'y': y}
        self.G = 0
        self.H = 0
        self.F = 0
        self.prev = None
        self.type = space_enum["empty"]
        self.cost = 1

class Pathing:
    
    def __init__(self, map_data: dict):
        self.map_data = map_data
        self.open_list = {}
        self.closed_list = {}
    
    def find_path(self, start: Node, end: Node):
        current_node = start
        while current_node.position != end.position:
            self.find_open_paths(current_node, end)
            key, node = self.find_best_path()
            self.open_list.pop(key)
            self.closed_list[key] = node
            current_node = node
        
        cursor = end
        path = [end]
        while cursor.position != start.position:
            cursor = self.closed_list[(cursor.position['x'],cursor.position['y'])].prev
            path.append(cursor)
        # Reverse the path ::-1
        return path[::-1]

    def find_open_paths(self, current_node, end_node):

        cardinal_directions = [{'x': 0, 'y': 1}, {'x': 1, 'y': 0}, {'x': 0, 'y': -1}, {'x': -1, 'y': 0}]

        for direction in cardinal_directions:
            new_x = current_node.position['x'] + direction['x']
            new_y = current_node.position['y'] + direction['y']
            new_location = (new_x, new_y)

            if (new_location in self.map_data
                and new_location not in self.closed_list):

                neighbor = self.map_data[new_location]
                
                if neighbor.type != space_enum['block']:

                    G = neighbor.cost + current_node.G
                    H = math.sqrt((end_node.position['x'] - new_x) ** 2 + (end_node.position['y'] - new_y) ** 2)
                    F = G + H

                    # Is the position in the open list and if so is its F < than the current position's F
                    if new_location in self.open_list:
                        if F < self.open_list[new_location].F:
                            neighbor.G = G
                            neighbor.H = H
                            neighbor.F = F
                            neighbor.prev = current_node
                            self.open_list[new_location] = neighbor
                    else:
                        neighbor.G = G
                        neighbor.H = H
                        neighbor.F = F
                        neighbor.prev = current_node
                        self.open_list[new_location] = neighbor

    def find_best_path(self):
        smallest_f = float('inf')
        smallest_key = None
        smallest_node = None

        for key, node in self.open_list.items():
            if node.F < smallest_f:
                smallest_f = node.F
                smallest_key = key
                smallest_node = node

        return smallest_key, smallest_node



def generate_map(blocked):
    gen_map = {}
    for x in range(5):
        for y in range(5):
            gen_map[(x, y)] = Node(x, y)
            if ((x,y) in blocked):
                gen_map[(x, y)].type = space_enum['block'] 
    return gen_map

map_data = generate_map([(0,3),(1,2)])

player_node = map_data[(0, 0)]
player_node.type = space_enum["player"]
end_node = map_data[(0, 4)]

player_pathing = Pathing(map_data)
# Feed (0,0):player_node (the start location) into closed list
player_pathing.closed_list = {(0,0):player_node} 
path = player_pathing.find_path(player_node, end_node)

for n in path:
    print(n.position)