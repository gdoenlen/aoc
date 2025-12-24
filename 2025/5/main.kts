import java.nio.file.Files
import kotlin.io.path.Path

class Range(val start: Long, val end: Long) {
  fun isWithin(l: Long) = this.start <= l && l <= this.end
}

class Ingredients(private val ranges: List<Range>) {
  fun isFresh(id: Long) = this.ranges.find { it.isWithin(id) } != null
}

val ingredients = Ingredients(
  Files.readAllLines(Path("./ranges.txt"))
    .asSequence()
    .map(::toRange)
    .sortedBy(Range::start)
    .toList()
)

fun toRange(line: String): Range {
  val parts = line.split("-")
  return Range(parts[0].toLong(), parts[1].toLong())
}

val sum = Files.readAllLines(Path("./ids.txt"))
  .asSequence()
  .map { it.toLong() }
  .filter(ingredients::isFresh)
  .count()
println(sum)
