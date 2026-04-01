import { useEffect, useState } from 'react';
import { getDashboardSummary } from '../api/orderApi';
import type { DashboardSummary } from '../api/orderApi';
import { Link } from 'react-router-dom';

export default function Dashboard() {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);

  useEffect(() => {
    getDashboardSummary().then(setSummary);
    const interval = setInterval(() => getDashboardSummary().then(setSummary), 5000);
    return () => clearInterval(interval);
  }, []);

  if (!summary) return <p>Loading...</p>;

  return (
    <div>
      <h2>Admin Dashboard</h2>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '1rem', marginTop: '1rem' }}>
        <Card title="Total Orders" value={summary.totalOrders} color="#3498db" />
        <Card title="Pending" value={summary.pendingOrders} color="#f39c12" />
        <Card title="Completed" value={summary.completedOrders} color="#2ecc71" />
        <Card title="Failed" value={summary.failedOrders} color="#e74c3c" />
      </div>
      <div style={{ marginTop: '1rem', padding: '1rem', background: '#f8f9fa', borderRadius: '8px' }}>
        <h4>Total Revenue: ${summary.totalRevenue.toFixed(2)}</h4>
      </div>
      <div style={{ marginTop: '1rem', display: 'flex', gap: '0.5rem' }}>
        <Link to="/orders" style={{ padding: '0.5rem 1rem', background: '#3498db', color: 'white', borderRadius: '4px', textDecoration: 'none' }}>View All Orders</Link>
        <Link to="/orders/failed" style={{ padding: '0.5rem 1rem', background: '#e74c3c', color: 'white', borderRadius: '4px', textDecoration: 'none' }}>View Failed Orders</Link>
      </div>
    </div>
  );
}

function Card({ title, value, color }: { title: string; value: number; color: string }) {
  return (
    <div style={{ background: 'white', border: `2px solid ${color}`, borderRadius: '8px', padding: '1.5rem', textAlign: 'center' }}>
      <div style={{ fontSize: '2rem', fontWeight: 'bold', color }}>{value}</div>
      <div style={{ color: '#666', marginTop: '0.5rem' }}>{title}</div>
    </div>
  );
}
