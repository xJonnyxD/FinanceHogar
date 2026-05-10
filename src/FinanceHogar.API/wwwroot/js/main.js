// ============================================================
// FinanceHogar — Alpine.js App
// ============================================================

document.addEventListener('alpine:init', () => {

  // ── Global Store ─────────────────────────────────────────
  Alpine.store('fh', {
    user:    api.getUser(),
    hogarId: api.getHogarId(),
    hogar:   null,
    page:    api.isAuthenticated() ? 'dashboard' : 'login',
    toast:   null,
    alertasNoLeidas: 0,

    nav(page) {
      this.page = page;
      window.dispatchEvent(new CustomEvent('fh:page', { detail: page }));
    },

    showToast(msg, type = 'success') {
      this.toast = { msg, type };
      setTimeout(() => this.toast = null, 3500);
    },

    async loadHogar() {
      if (!this.hogarId) return;
      try { this.hogar = await api.getHogar(this.hogarId); } catch {}
    },

    async loadAlertasCount() {
      if (!this.hogarId) return;
      try {
        const list = await api.getAlertas(this.hogarId);
        this.alertasNoLeidas = Array.isArray(list) ? list.filter(a => a.estado !== 'Leida').length : 0;
      } catch {}
    }
  });

  window.addEventListener('fh:logout', () => {
    const s = Alpine.store('fh');
    s.user = null; s.hogarId = null; s.hogar = null; s.page = 'login';
  });

  // ─────────────────────────────────────────────────────────
  // Helpers
  // ─────────────────────────────────────────────────────────
  function onPage(pageName, fn) {
    window.addEventListener('fh:page', e => { if (e.detail === pageName) fn(); });
    if (Alpine.store('fh').page === pageName) fn();
  }

  // ─────────────────────────────────────────────────────────
  // ── AUTH COMPONENT ────────────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('authPage', () => ({
    tab: 'login',
    loading: false,
    error: '',
    loginEmail: '', loginPass: '',
    regNombre: '', regEmail: '', regPass: '', regTel: '', regDUI: '', regHogar: '',

    async doLogin() {
      this.error = ''; this.loading = true;
      try {
        const resp = await api.login(this.loginEmail, this.loginPass);
        const store = Alpine.store('fh');
        store.user    = api.getUser();
        store.hogarId = resp.hogarId ?? api.getHogarId();
        await store.loadHogar();
        await store.loadAlertasCount();
        store.nav('dashboard');
      } catch (e) {
        this.error = e.message;
      } finally {
        this.loading = false;
      }
    },

    async doRegister() {
      this.error = ''; this.loading = true;
      try {
        const resp = await api.register({
          nombreCompleto: this.regNombre,
          email:          this.regEmail,
          password:       this.regPass,
          telefono:       this.regTel   || undefined,
          dUI:            this.regDUI   || undefined,
          nombreHogar:    this.regHogar
        });
        const store = Alpine.store('fh');
        store.user    = api.getUser();
        store.hogarId = resp.hogarId ?? api.getHogarId();
        await store.loadHogar();
        store.nav('dashboard');
        store.showToast('¡Cuenta creada exitosamente! Bienvenido/a 🎉');
      } catch (e) {
        this.error = e.message;
      } finally {
        this.loading = false;
      }
    }
  }));

  // ─────────────────────────────────────────────────────────
  // ── DASHBOARD COMPONENT ──────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('dashboard', () => ({
    loading: false,
    balance: null, puntaje: null, tendencias: null,
    anio: new Date().getFullYear(),
    mes:  new Date().getMonth() + 1,
    chart: null, trendChart: null,

    init() { onPage('dashboard', () => this.load()); },

    async load() {
      this.loading = true;
      const hId = Alpine.store('fh').hogarId;
      if (!hId) { this.loading = false; return; }
      try {
        [this.balance, this.puntaje, this.tendencias] = await Promise.all([
          api.getBalance(hId, this.anio, this.mes),
          api.getPuntaje(hId),
          api.getTendencias(hId)
        ]);
        await Alpine.store('fh').loadHogar();
        this.$nextTick(() => this.drawCharts());
      } catch(e) {
        Alpine.store('fh').showToast('Error cargando dashboard: ' + e.message, 'danger');
      } finally { this.loading = false; }
    },

    get mesNombre() {
      return new Date(this.anio, this.mes - 1).toLocaleString('es-SV', { month: 'long', year: 'numeric' });
    },
    get scoreClass() { return this.puntaje?.nivel?.toLowerCase() || ''; },
    fmt(v) { return '$' + (v ?? 0).toLocaleString('es-SV', { minimumFractionDigits: 2 }); },

    drawCharts() { this.drawDonut(); this.drawTrend(); },

    drawDonut() {
      const ctx = document.getElementById('chartGastos');
      if (!ctx || !this.balance?.gastosPorCategoria?.length) return;
      if (this.chart) this.chart.destroy();
      this.chart = new Chart(ctx, {
        type: 'doughnut',
        data: {
          labels: this.balance.gastosPorCategoria.map(g => g.nombreCategoria),
          datasets: [{ data: this.balance.gastosPorCategoria.map(g => g.total),
            backgroundColor: ['#e63946','#f4a261','#2a9d8f','#457b9d','#9b2226','#6a4c93','#1d3557','#2ec4b6','#e9c46a','#264653'],
            borderWidth: 2, borderColor: '#fff' }]
        },
        options: { plugins: { legend: { position: 'right', labels: { font: { size: 11 } } } }, cutout: '68%', maintainAspectRatio: false }
      });
    },

    drawTrend() {
      const ctx = document.getElementById('chartTendencias');
      if (!ctx || !this.tendencias?.length) return;
      if (this.trendChart) this.trendChart.destroy();
      this.trendChart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: this.tendencias.map(t => new Date(t.anio, t.mes-1).toLocaleString('es-SV',{month:'short'})),
          datasets: [
            { label: 'Ingresos', data: this.tendencias.map(t => t.ingresos), backgroundColor: '#00a878', borderRadius: 6 },
            { label: 'Gastos',   data: this.tendencias.map(t => t.gastos),   backgroundColor: '#e63946', borderRadius: 6 }
          ]
        },
        options: {
          plugins: { legend: { position: 'top' } },
          scales: { y: { beginAtZero: true, ticks: { callback: v => '$'+v.toLocaleString() } } },
          maintainAspectRatio: false
        }
      });
    }
  }));

  // ─────────────────────────────────────────────────────────
  // ── GASTOS COMPONENT ─────────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('gastosPage', () => ({
    loading: false, items: [], categorias: [],
    modal: null, form: {}, editing: null,
    filterDesde: '', filterHasta: '',

    init() { onPage('gastos', () => this.load()); },

    async load() {
      this.loading = true;
      const hId = Alpine.store('fh').hogarId;
      try {
        const [g, c] = await Promise.all([api.getGastos(hId), api.getCategorias(hId)]);
        this.items      = Array.isArray(g) ? g : [];
        this.categorias = Array.isArray(c) ? c.filter(x => !x.esIngreso) : [];
      } catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    async applyFilter() {
      const hId = Alpine.store('fh').hogarId;
      const p = {};
      if (this.filterDesde) p.desde = this.filterDesde;
      if (this.filterHasta) p.hasta = this.filterHasta;
      this.loading = true;
      try { this.items = await api.getGastos(hId, p); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    openCreate() {
      const today = new Date().toISOString().split('T')[0];
      this.form = { hogarId: Alpine.store('fh').hogarId, categoriaId: '', monto: '', moneda: 'USD', tipo: 1, descripcion: '', fechaGasto: today, esRecurrente: false, frecuencia: null };
      this.modal = 'create';
    },
    openEdit(item) {
      this.editing = item;
      this.form = { hogarId: item.hogarId, categoriaId: item.categoriaId, monto: item.monto, moneda: item.moneda, tipo: item.tipo, descripcion: item.descripcion, fechaGasto: item.fechaGasto, esRecurrente: item.esRecurrente, frecuencia: item.frecuencia };
      this.modal = 'edit';
    },
    async save() {
      const store = Alpine.store('fh');
      try {
        const payload = { ...this.form, monto: parseFloat(this.form.monto) };
        if (this.modal === 'create') {
          this.items.unshift(await api.createGasto(payload));
          store.showToast('Gasto registrado ✓');
        } else {
          const act = await api.updateGasto(this.editing.id, payload);
          const idx = this.items.findIndex(g => g.id === this.editing.id);
          if (idx >= 0) this.items[idx] = act;
          store.showToast('Gasto actualizado ✓');
        }
        this.modal = null;
        await store.loadAlertasCount();
      } catch(e) { store.showToast(e.message, 'danger'); }
    },
    async remove(id) {
      if (!confirm('¿Eliminar este gasto?')) return;
      try { await api.deleteGasto(id); this.items = this.items.filter(g => g.id !== id); Alpine.store('fh').showToast('Gasto eliminado'); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },
    fmt(v) { return '$' + (+v).toLocaleString('es-SV', { minimumFractionDigits: 2 }); },
    fmtDate(d) { return d ? new Date(d+'T00:00:00').toLocaleDateString('es-SV') : '—'; }
  }));

  // ─────────────────────────────────────────────────────────
  // ── INGRESOS COMPONENT ───────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('ingresosPage', () => ({
    loading: false, items: [], categorias: [],
    modal: null, form: {}, editing: null, resumen: null,

    init() { onPage('ingresos', () => this.load()); },

    async load() {
      this.loading = true;
      const hId = Alpine.store('fh').hogarId;
      const anio = new Date().getFullYear(), mes = new Date().getMonth() + 1;
      try {
        const [i, c, r] = await Promise.all([api.getIngresos(hId), api.getCategorias(hId), api.getResumenIngresos(hId, anio, mes)]);
        this.items      = Array.isArray(i) ? i : [];
        this.categorias = Array.isArray(c) ? c.filter(x => x.esIngreso) : [];
        this.resumen    = r;
      } catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    openCreate() {
      const today = new Date().toISOString().split('T')[0];
      this.form = { hogarId: Alpine.store('fh').hogarId, categoriaId: '', monto: '', moneda: 'USD', tipo: 1, descripcion: '', fechaIngreso: today, esRecurrente: false, frecuencia: null };
      this.modal = 'create';
    },
    openEdit(item) {
      this.editing = item;
      this.form = { hogarId: item.hogarId, categoriaId: item.categoriaId, monto: item.monto, moneda: item.moneda, tipo: item.tipo, descripcion: item.descripcion, fechaIngreso: item.fechaIngreso, esRecurrente: item.esRecurrente, frecuencia: item.frecuencia };
      this.modal = 'edit';
    },
    async save() {
      const store = Alpine.store('fh');
      try {
        const payload = { ...this.form, monto: parseFloat(this.form.monto) };
        if (this.modal === 'create') {
          this.items.unshift(await api.createIngreso(payload));
          store.showToast('Ingreso registrado ✓');
        } else {
          const act = await api.updateIngreso(this.editing.id, payload);
          const idx = this.items.findIndex(i => i.id === this.editing.id);
          if (idx >= 0) this.items[idx] = act;
          store.showToast('Ingreso actualizado ✓');
        }
        this.modal = null;
      } catch(e) { store.showToast(e.message, 'danger'); }
    },
    async remove(id) {
      if (!confirm('¿Eliminar este ingreso?')) return;
      try { await api.deleteIngreso(id); this.items = this.items.filter(i => i.id !== id); Alpine.store('fh').showToast('Ingreso eliminado'); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },
    fmt(v) { return '$' + (+v).toLocaleString('es-SV', { minimumFractionDigits: 2 }); },
    fmtDate(d) { return d ? new Date(d+'T00:00:00').toLocaleDateString('es-SV') : '—'; }
  }));

  // ─────────────────────────────────────────────────────────
  // ── PRESUPUESTOS COMPONENT ───────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('presupuestosPage', () => ({
    loading: false, items: [], comparativo: [], categorias: [],
    modal: null, form: {}, editing: null,
    anio: new Date().getFullYear(), mes: new Date().getMonth() + 1,

    init() { onPage('presupuestos', () => this.cargar()); },

    async cargar() {
      this.loading = true;
      const hId = Alpine.store('fh').hogarId;
      try {
        const [p, comp, c] = await Promise.all([api.getPresupuestos(hId, this.anio, this.mes), api.getComparativo(hId, this.anio, this.mes), api.getCategorias(hId)]);
        this.items      = Array.isArray(p)    ? p    : [];
        this.comparativo= Array.isArray(comp) ? comp : [];
        this.categorias = Array.isArray(c)    ? c.filter(x => !x.esIngreso) : [];
      } catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    openCreate() {
      this.form = { hogarId: Alpine.store('fh').hogarId, categoriaId: '', anio: this.anio, mes: this.mes, montoLimite: '' };
      this.modal = 'create';
    },
    openEdit(item) { this.editing = item; this.form = { montoLimite: item.montoLimite }; this.modal = 'edit'; },
    async save() {
      const store = Alpine.store('fh');
      try {
        if (this.modal === 'create') {
          this.items.push(await api.createPresupuesto({ ...this.form, montoLimite: parseFloat(this.form.montoLimite) }));
          store.showToast('Presupuesto creado ✓');
        } else {
          const act = await api.updatePresupuesto(this.editing.id, { montoLimite: parseFloat(this.form.montoLimite) });
          const idx = this.items.findIndex(p => p.id === this.editing.id);
          if (idx >= 0) this.items[idx] = act;
          store.showToast('Presupuesto actualizado ✓');
        }
        this.modal = null; await this.cargar();
      } catch(e) { store.showToast(e.message, 'danger'); }
    },
    async remove(id) {
      if (!confirm('¿Eliminar este presupuesto?')) return;
      try { await api.deletePresupuesto(id); this.items = this.items.filter(p => p.id !== id); Alpine.store('fh').showToast('Presupuesto eliminado'); await this.cargar(); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },
    fmt(v) { return '$' + (+v).toLocaleString('es-SV', { minimumFractionDigits: 2 }); }
  }));

  // ─────────────────────────────────────────────────────────
  // ── ALERTAS COMPONENT ────────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('alertasPage', () => ({
    loading: false, items: [],

    init() { onPage('alertas', () => this.load()); },

    async load() {
      this.loading = true;
      try { this.items = await api.getAlertas(Alpine.store('fh').hogarId) ?? []; }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    async marcarLeida(id) {
      try {
        await api.marcarLeida(id);
        const it = this.items.find(a => a.id === id);
        if (it) it.estado = 'Leida';
        await Alpine.store('fh').loadAlertasCount();
        Alpine.store('fh').showToast('Marcada como leída ✓');
      } catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },

    async remove(id) {
      if (!confirm('¿Eliminar alerta?')) return;
      try { await api.deleteAlerta(id); this.items = this.items.filter(a => a.id !== id); Alpine.store('fh').showToast('Alerta eliminada'); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },

    iconoTipo(tipo) {
      return ({ 'Advertencia':'bi-exclamation-triangle-fill text-warning', 'Alerta':'bi-exclamation-circle-fill text-danger', 'Critico':'bi-x-octagon-fill text-danger', 'ServicioBasico':'bi-lightning-charge-fill text-warning' })[tipo] ?? 'bi-bell-fill text-secondary';
    },
    fmtDate(d) { return d ? new Date(d).toLocaleDateString('es-SV') : '—'; }
  }));

  // ─────────────────────────────────────────────────────────
  // ── SERVICIOS BÁSICOS COMPONENT ──────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('serviciosPage', () => ({
    loading: false, items: [], proximos: [],
    modal: null, form: {}, editing: null,
    TIPOS: [{v:1,l:'Agua'},{v:2,l:'Electricidad'},{v:3,l:'Internet'},{v:4,l:'Teléfono'},{v:5,l:'Gas'},{v:6,l:'Cable TV'},{v:7,l:'Otro'}],

    init() { onPage('servicios', () => this.load()); },

    async load() {
      this.loading = true;
      const hId = Alpine.store('fh').hogarId;
      try {
        const [s, v] = await Promise.all([api.getServicios(hId), api.getVencimientos(hId)]);
        this.items    = Array.isArray(s) ? s : [];
        this.proximos = Array.isArray(v) ? v : [];
      } catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    openCreate() {
      const hoy = new Date(); hoy.setDate(hoy.getDate() + 30);
      this.form = { hogarId: Alpine.store('fh').hogarId, tipoServicio: 1, nombreProveedor: '', montoPromedio: '', fechaVencimiento: hoy.toISOString().split('T')[0], diasAnticipacionNotificacion: 5 };
      this.modal = 'create';
    },
    openEdit(item) {
      this.editing = item;
      this.form = { nombreProveedor: item.nombreProveedor, montoPromedio: item.montoPromedio, fechaVencimiento: item.fechaVencimiento, diasAnticipacionNotificacion: item.diasAnticipacionNotificacion };
      this.modal = 'edit';
    },
    async save() {
      const store = Alpine.store('fh');
      try {
        const payload = { ...this.form, montoPromedio: parseFloat(this.form.montoPromedio) };
        if (this.modal === 'create') {
          this.items.unshift(await api.createServicio(payload));
          store.showToast('Servicio creado ✓');
        } else {
          const act = await api.updateServicio(this.editing.id, payload);
          const idx = this.items.findIndex(s => s.id === this.editing.id);
          if (idx >= 0) this.items[idx] = act;
          store.showToast('Servicio actualizado ✓');
        }
        this.modal = null;
      } catch(e) { store.showToast(e.message, 'danger'); }
    },
    async remove(id) {
      if (!confirm('¿Eliminar este servicio?')) return;
      try { await api.deleteServicio(id); this.items = this.items.filter(s => s.id !== id); Alpine.store('fh').showToast('Servicio eliminado'); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },
    tipoLabel(t) { return this.TIPOS.find(x => x.v == t)?.l ?? t; },
    fmt(v) { return '$' + (+v).toLocaleString('es-SV', { minimumFractionDigits: 2 }); },
    fmtDate(d) { return d ? new Date(d+'T00:00:00').toLocaleDateString('es-SV') : '—'; }
  }));

  // ─────────────────────────────────────────────────────────
  // ── TANDAS COMPONENT ─────────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('tandasPage', () => ({
    loading: false, items: [], modal: null, form: {}, editing: null,

    init() { onPage('tandas', () => this.load()); },

    async load() {
      this.loading = true;
      try { this.items = await api.getTandas(Alpine.store('fh').hogarId) ?? []; }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    openCreate() {
      const today = new Date().toISOString().split('T')[0];
      this.form = { hogarId: Alpine.store('fh').hogarId, nombre: '', cuotaMensual: '', totalParticipantes: 2, fechaInicio: today };
      this.modal = 'create';
    },
    openEdit(item) { this.editing = item; this.form = { nombre: item.nombre, cuotaMensual: item.cuotaMensual }; this.modal = 'edit'; },
    async save() {
      const store = Alpine.store('fh');
      try {
        if (this.modal === 'create') {
          this.items.unshift(await api.createTanda({ ...this.form, cuotaMensual: parseFloat(this.form.cuotaMensual), totalParticipantes: parseInt(this.form.totalParticipantes) }));
          store.showToast('Tanda creada ✓');
        } else {
          const act = await api.updateTanda(this.editing.id, { nombre: this.form.nombre, cuotaMensual: parseFloat(this.form.cuotaMensual) });
          const idx = this.items.findIndex(t => t.id === this.editing.id);
          if (idx >= 0) this.items[idx] = act;
          store.showToast('Tanda actualizada ✓');
        }
        this.modal = null;
      } catch(e) { store.showToast(e.message, 'danger'); }
    },
    async remove(id) {
      if (!confirm('¿Eliminar esta tanda?')) return;
      try { await api.deleteTanda(id); this.items = this.items.filter(t => t.id !== id); Alpine.store('fh').showToast('Tanda eliminada'); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },
    estadoClass(e) { return 'badge ' + ({'Activa':'badge-activa','Completada':'badge-pagado','Cancelada':'badge-gasto'}[e] ?? 'bg-secondary text-white'); },
    fmt(v) { return '$' + (+v).toLocaleString('es-SV', { minimumFractionDigits: 2 }); },
    fmtDate(d) { return d ? new Date(d+'T00:00:00').toLocaleDateString('es-SV') : '—'; }
  }));

  // ─────────────────────────────────────────────────────────
  // ── REMESAS COMPONENT ────────────────────────────────────
  // ─────────────────────────────────────────────────────────
  Alpine.data('remesasPage', () => ({
    loading: false, items: [], categorias: [], modal: null, form: {}, editing: null,

    init() { onPage('remesas', () => this.load()); },

    async load() {
      this.loading = true;
      const hId = Alpine.store('fh').hogarId;
      try {
        const [r, c] = await Promise.all([api.getRemesas(hId), api.getCategorias(hId)]);
        this.items      = Array.isArray(r) ? r : [];
        this.categorias = Array.isArray(c) ? c.filter(x => x.esIngreso) : [];
      } catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
      finally { this.loading = false; }
    },

    openCreate() {
      const today = new Date().toISOString().split('T')[0];
      const cat = this.categorias.find(c => c.nombre.toLowerCase().includes('remesa'));
      this.form = { hogarId: Alpine.store('fh').hogarId, categoriaId: cat?.id ?? '', monto: '', moneda: 'USD', paisOrigen: 'Estados Unidos', empresa: '', numeroConfirmacion: '', fechaRecepcion: today };
      this.modal = 'create';
    },
    openEdit(item) {
      this.editing = item;
      this.form = { monto: item.monto, empresa: item.empresa, numeroConfirmacion: item.numeroConfirmacion, fechaRecepcion: item.fechaRecepcion };
      this.modal = 'edit';
    },
    async save() {
      const store = Alpine.store('fh');
      try {
        if (this.modal === 'create') {
          this.items.unshift(await api.createRemesa({ ...this.form, monto: parseFloat(this.form.monto) }));
          store.showToast('Remesa registrada ✓');
        } else {
          const act = await api.updateRemesa(this.editing.id, { monto: parseFloat(this.form.monto), empresa: this.form.empresa, numeroConfirmacion: this.form.numeroConfirmacion, fechaRecepcion: this.form.fechaRecepcion });
          const idx = this.items.findIndex(r => r.id === this.editing.id);
          if (idx >= 0) this.items[idx] = act;
          store.showToast('Remesa actualizada ✓');
        }
        this.modal = null;
      } catch(e) { store.showToast(e.message, 'danger'); }
    },
    async remove(id) {
      if (!confirm('¿Eliminar esta remesa?')) return;
      try { await api.deleteRemesa(id); this.items = this.items.filter(r => r.id !== id); Alpine.store('fh').showToast('Remesa eliminada'); }
      catch(e) { Alpine.store('fh').showToast(e.message, 'danger'); }
    },
    fmt(v) { return '$' + (+v).toLocaleString('es-SV', { minimumFractionDigits: 2 }); },
    fmtDate(d) { return d ? new Date(d+'T00:00:00').toLocaleDateString('es-SV') : '—'; }
  }));

});
