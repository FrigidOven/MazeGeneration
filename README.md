This is a random maze generator I wrote using an algorithm I designed. I am unsure if this algorithm
is already known or not but it can generate mazes just by iterating over the grid like any other two
dimensional array. It assigns values as necessary to each grid cell and its neighbors if need be.
Each value in the grid is a 4 bit binary number wherein the most significant bit represents a connection
leading north, the second most significant bit is a connection leading east, the third a southern connection,
and the least significant bit represents a western connection.

Results:
![image](https://github.com/user-attachments/assets/693b4606-3346-44cf-b56f-dfbce16f43e7)


When solving the generated mazes, start at the top left and try to reach the bottom right.
