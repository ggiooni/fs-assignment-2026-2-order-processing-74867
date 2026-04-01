import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import OrdersTable from './pages/OrdersTable';
import OrderDetails from './pages/OrderDetails';
import FailedOrders from './pages/FailedOrders';

function App() {
  return (
    <BrowserRouter>
      <div style={{ display: 'flex', minHeight: '100vh' }}>
        <nav style={{ width: '220px', background: '#2c3e50', color: 'white', padding: '1rem' }}>
          <h3 style={{ marginTop: 0 }}>Admin Panel</h3>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            <Link to="/" style={{ color: '#ecf0f1', textDecoration: 'none', padding: '0.5rem', borderRadius: '4px' }}>Dashboard</Link>
            <Link to="/orders" style={{ color: '#ecf0f1', textDecoration: 'none', padding: '0.5rem', borderRadius: '4px' }}>Orders</Link>
            <Link to="/orders/failed" style={{ color: '#ecf0f1', textDecoration: 'none', padding: '0.5rem', borderRadius: '4px' }}>Failed Orders</Link>
          </div>
        </nav>
        <main style={{ flex: 1, padding: '2rem', background: '#f5f6fa' }}>
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/orders" element={<OrdersTable />} />
            <Route path="/orders/failed" element={<FailedOrders />} />
            <Route path="/orders/:id" element={<OrderDetails />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;
