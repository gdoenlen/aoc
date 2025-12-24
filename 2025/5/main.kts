import java.nio.file.Files
import kotlin.io.path.Path

@JvmRecord
data class Range(val start: Long, val end: Long) {
  fun contains(l: Long) = l in start..end
  fun count() = end - (start - 1)
  fun isWithin(other: Range) = this.contains(other.start) && this.contains(other.end)
  fun isOverlapping(other: Range) = this.contains(other.start)
}

class Ingredients(private val ranges: List<Range>) {
  fun isFresh(id: Long) = this.ranges.find { it.contains(id) } != null
  fun count(): Long {
    // the ranges can overlap each other and will cause overflow if you count them
    // they're sorted by start index below.
    var count = this.ranges.first().count()
    var previous = this.ranges.first()
    for (range in this.ranges.listIterator(1)) {
      if (previous.isWithin(range))
        continue
      count += if (previous.isOverlapping(range)) {
        Range(previous.end + 1, range.end).count()
      } else {
        range.count()
      }
      previous = range
    }

    return count
  }
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
  val start = parts[0].toLong()
  val end = parts[1].toLong()

  return Range(start, end)
}

val sum = Files.readAllLines(Path("./ids.txt"))
  .asSequence()
  .map(String::toLong)
  .filter(ingredients::isFresh)
  .count()

println(sum)
println(ingredients.count())
