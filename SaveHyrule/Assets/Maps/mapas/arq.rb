input_file = ARGV[0]
output_class = ARGV[1]
map_type = ARGV[2]
output_file = output_class + ".cs"

finalarq = ""

finalarq = finalarq + "using UnityEngine;\n"
finalarq = finalarq + "using System.Collections;\n"
finalarq = finalarq + "using System.Collections.Generic;\n"
finalarq = finalarq + "\n"
finalarq = finalarq + "public class "+output_class+" : Map {\n"
finalarq = finalarq + "public "+output_class+"() { \n"
finalarq = finalarq + "map = new List<List<char>> {\n"

is_dungeon = false

if (map_type == "dungeon")
  is_dungeon = true
end


File.readlines(input_file).each do |l|
  x = l.split
  string_total = ""
  
  x.each do |a|
    if(is_dungeon)
      if(a == '0')
        string_total = string_total + "'X',"
      else
        string_total = string_total + "'D',"
      end
    else
      string_total = string_total + "'#{a}',"
    end
    
  end

  finalarq = finalarq + "new List<char> { #{string_total[0..(string_total.length() - 2)]} },\n"
end

finalfarq = finalarq[0..(finalarq.length()-3)]
finalarq = finalarq + "};\n"
finalarq = finalarq + "}\n"
finalarq = finalarq + "}\n"

File.write(output_file, finalarq)
