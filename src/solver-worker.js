// Web Worker for brute force funding solver calculation

self.addEventListener('message', function(e) {
  const { price022, price023, price024, totalCost, maxQty } = e.data;

  console.log('Worker: Starting calculation...');

  const solutions = [];

  for (let x = 0; x <= maxQty; x++) {
    for (let y = 0; y <= maxQty; y++) {
      for (let z = 0; z <= maxQty; z++) {
        const total = (price022 * x) + (price023 * y) + (price024 * z);
        if (Math.abs(total - totalCost) < 0.01) { // Float comparison with epsilon
          solutions.push([x, y, z]);
        }
      }
    }
  }

  console.log(`Worker: Found ${solutions.length} solutions`);

  if (solutions.length === 0) {
    self.postMessage({ type: 'NoSolutions' });
  } else {
    self.postMessage({ type: 'Solutions', solutions: solutions });
  }
});
