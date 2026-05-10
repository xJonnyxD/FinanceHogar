// ============================================================
// FinanceHogar — API Client
// ============================================================
const API_BASE = '/api/v1';

const api = {
  // ── Auth helpers ──────────────────────────────────────────
  getToken() { return localStorage.getItem('fh_token'); },
  getUser()  { try { return JSON.parse(localStorage.getItem('fh_user')); } catch { return null; } },
  getHogarId() { return localStorage.getItem('fh_hogarId'); },

  setAuth(resp, hogarId) {
    localStorage.setItem('fh_token',   resp.token);
    localStorage.setItem('fh_refresh', resp.refreshToken);
    localStorage.setItem('fh_user',    JSON.stringify({ id: resp.usuarioId, nombre: resp.nombreCompleto, email: resp.email }));
    if (hogarId) localStorage.setItem('fh_hogarId', hogarId);
  },

  clearAuth() {
    ['fh_token','fh_refresh','fh_user','fh_hogarId'].forEach(k => localStorage.removeItem(k));
  },

  isAuthenticated() { return !!this.getToken(); },

  // ── Core request ─────────────────────────────────────────
  async request(method, path, body = null) {
    const headers = { 'Content-Type': 'application/json' };
    const token = this.getToken();
    if (token) headers['Authorization'] = `Bearer ${token}`;

    const options = { method, headers };
    if (body !== null) options.body = JSON.stringify(body);

    let res;
    try {
      res = await fetch(`${API_BASE}${path}`, options);
    } catch (e) {
      throw new Error('No se pudo conectar al servidor. Verifica que la API esté corriendo.');
    }

    if (res.status === 401) {
      this.clearAuth();
      window.dispatchEvent(new CustomEvent('fh:logout'));
      throw new Error('Sesión expirada. Por favor inicia sesión de nuevo.');
    }
    if (res.status === 204) return null;

    let data;
    try { data = await res.json(); } catch { data = {}; }

    if (!res.ok) {
      const msg = data?.errors
        ? Object.values(data.errors).flat().join(' | ')
        : (data?.message ?? data?.title ?? `Error ${res.status}`);
      throw new Error(msg);
    }
    return data;
  },

  get:    (path)        => api.request('GET',    path),
  post:   (path, body)  => api.request('POST',   path, body),
  put:    (path, body)  => api.request('PUT',    path, body),
  delete: (path)        => api.request('DELETE', path),

  // ── Auth ─────────────────────────────────────────────────
  async login(email, password) {
    const resp = await this.post('/auth/login', { email, password });
    // Guardar token y auth con hogarId desde la respuesta
    this.setAuth(resp, resp.hogarId);
    return resp;
  },

  async register(data) {
    const resp = await this.post('/auth/register', data);
    // Guardar token y auth con hogarId desde la respuesta
    this.setAuth(resp, resp.hogarId);
    return resp;
  },

  logout() {
    const token = this.getToken();
    if (token) this.post('/auth/logout').catch(() => {});
    this.clearAuth();
  },

  // ── Hogares ───────────────────────────────────────────────
  getHogares:   ()       => api.get('/hogares'),
  getHogar:     (id)     => api.get(`/hogares/${id}`),
  updateHogar:  (id, d)  => api.put(`/hogares/${id}`, d),

  // ── Categorías ────────────────────────────────────────────
  getCategorias: (hogarId) => api.get(`/categorias${hogarId ? '?hogarId='+hogarId : ''}`),

  // ── Dashboard / Reportes ─────────────────────────────────
  getBalance:  (hogarId, anio, mes) =>
    api.get(`/reportes/balance-mensual?hogarId=${hogarId}&anio=${anio}&mes=${mes}`),
  getTendencias: (hogarId) =>
    api.get(`/reportes/tendencias?hogarId=${hogarId}`),
  getPuntaje:  (hogarId) =>
    api.get(`/reportes/puntaje-financiero?hogarId=${hogarId}`),

  // ── Gastos ────────────────────────────────────────────────
  getGastos: (hogarId, params = {}) => {
    const q = new URLSearchParams({ hogarId, ...params }).toString();
    return api.get(`/gastos?${q}`);
  },
  createGasto: (d) => api.post('/gastos', d),
  updateGasto: (id, d) => api.put(`/gastos/${id}`, d),
  deleteGasto: (id) => api.delete(`/gastos/${id}`),

  // ── Ingresos ─────────────────────────────────────────────
  getIngresos: (hogarId, params = {}) => {
    const q = new URLSearchParams({ hogarId, ...params }).toString();
    return api.get(`/ingresos?${q}`);
  },
  createIngreso: (d) => api.post('/ingresos', d),
  updateIngreso: (id, d) => api.put(`/ingresos/${id}`, d),
  deleteIngreso: (id) => api.delete(`/ingresos/${id}`),
  getResumenIngresos: (hogarId, anio, mes) =>
    api.get(`/ingresos/resumen-mensual?hogarId=${hogarId}&anio=${anio}&mes=${mes}`),

  // ── Presupuestos ─────────────────────────────────────────
  getPresupuestos: (hogarId, anio, mes) =>
    api.get(`/presupuestos?hogarId=${hogarId}&anio=${anio}&mes=${mes}`),
  getComparativo:  (hogarId, anio, mes) =>
    api.get(`/presupuestos/comparativo?hogarId=${hogarId}&anio=${anio}&mes=${mes}`),
  createPresupuesto: (d)      => api.post('/presupuestos', d),
  updatePresupuesto: (id, d)  => api.put(`/presupuestos/${id}`, d),
  deletePresupuesto: (id)     => api.delete(`/presupuestos/${id}`),

  // ── Alertas ───────────────────────────────────────────────
  getAlertas:   (hogarId) => api.get(`/alertas?hogarId=${hogarId}`),
  marcarLeida:  (id)      => api.put(`/alertas/${id}/leer`, {}),
  deleteAlerta: (id)      => api.delete(`/alertas/${id}`),

  // ── Servicios Básicos ────────────────────────────────────
  getServicios:   (hogarId) => api.get(`/serviciosbasicos?hogarId=${hogarId}`),
  getVencimientos:(hogarId) => api.get(`/serviciosbasicos/vencimientos?hogarId=${hogarId}`),
  createServicio: (d)       => api.post('/serviciosbasicos', d),
  updateServicio: (id, d)   => api.put(`/serviciosbasicos/${id}`, d),
  deleteServicio: (id)      => api.delete(`/serviciosbasicos/${id}`),

  // ── Tandas ────────────────────────────────────────────────
  getTandas:    (hogarId) => api.get(`/tandas?hogarId=${hogarId}`),
  createTanda:  (d)       => api.post('/tandas', d),
  updateTanda:  (id, d)   => api.put(`/tandas/${id}`, d),
  deleteTanda:  (id)      => api.delete(`/tandas/${id}`),

  // ── Remesas ───────────────────────────────────────────────
  getRemesas:    (hogarId) => api.get(`/remesas?hogarId=${hogarId}`),
  createRemesa:  (d)       => api.post('/remesas', d),
  updateRemesa:  (id, d)   => api.put(`/remesas/${id}`, d),
  deleteRemesa:  (id)      => api.delete(`/remesas/${id}`),
};
