<script setup lang="ts">
import { reactive, computed, onMounted, onUnmounted, watch, useTemplateRef } from "vue";
import { useNavigationBarStore } from "@/stores";

// Child components.
import NavigationBarItem from "./NavigationBarItem.vue";
import {
  HomeIcon as HomeOutlineIcon,
  UserCircleIcon as CustomerOutlineIcon,
  ArchiveBoxIcon as ProductOutlineIcon,
  TruckIcon as SupplyOutlineIcon,
  ShoppingCartIcon as OrderOutlineIcon,
  CurrencyDollarIcon as DebtOutlineIcon,
  CreditCardIcon as ExpenseOutlineIcon,
  ChartPieIcon as ReportOutlineIcon,
  IdentificationIcon as UserOutlineIcon } from "@heroicons/vue/24/outline";
import {
  HomeIcon as HomeSolidIcon,
  UserCircleIcon as CustomerSolidIcon,
  ArchiveBoxIcon as ProductSolidIcon,
  TruckIcon as SupplySolidIcon,
  ShoppingCartIcon as OrderSolidIcon,
  CurrencyDollarIcon as DebtSolidIcon,
  CreditCardIcon as ExpenseSolidIcon,
  ChartPieIcon as ReportSolidIcon,
  IdentificationIcon as UserSolidIcon } from "@heroicons/vue/24/solid";

// Dependencies.
const navigationBarStore = useNavigationBarStore();

// States.
const navigationBarElementRef = useTemplateRef<HTMLElement>("navigationBarElementRef");
const states = reactive({
  activeItemName: null as string | null,
  shouldBlockPointerEvent: false
});

// Computed.
const className = computed<string | null>(() => {
  if (navigationBarStore.isExpanded) {
    return "expanded";
  }

  return null;
});

// Callbacks.
function handleDocumentClicked(event: PointerEvent): void {
  if (!navigationBarStore.isExpanded) {
    return;
  }

  if (navigationBarElementRef.value === (event.target as HTMLElement)) {
    navigationBarStore.collapse();
  }
}

// Life cycle hooks.
onMounted(() => document.addEventListener("click", handleDocumentClicked));
onUnmounted(() => document.removeEventListener("click", handleDocumentClicked));

// Watch.
watch(() => navigationBarStore.isExpanded, () => {
  if (navigationBarStore.isExpanded) {
    states.shouldBlockPointerEvent = true;
    setTimeout(() => states.shouldBlockPointerEvent = true, 200);
  }
});
</script>

<template>
  <nav id="navbar" v-bind:class="className" ref="navigationBarElementRef">
    <div id="navbar-container">
      <div id="navbar-item-list">
        <!-- Home -->
        <NavigationBarItem name="home" fallback-display-name="Trang chủ" v-slot="{ isActive }">
          <HomeSolidIcon v-if="isActive" />
          <HomeOutlineIcon v-else />
        </NavigationBarItem>

        <!-- Customer -->
        <NavigationBarItem name="customer" v-slot="{ isActive }">
          <CustomerSolidIcon v-if="isActive" />
          <CustomerOutlineIcon v-else />
        </NavigationBarItem>
        
        <!-- Product -->
        <NavigationBarItem name="product" v-slot="{ isActive }">
          <ProductSolidIcon v-if="isActive" />
          <ProductOutlineIcon v-else />
        </NavigationBarItem>

        <!-- Supply -->
        <NavigationBarItem name="supply" v-slot="{ isActive }">
          <SupplySolidIcon v-if="isActive" />
          <SupplyOutlineIcon v-else />
        </NavigationBarItem>

        <!-- Order -->
        <NavigationBarItem name="order" v-slot="{ isActive }">
          <OrderSolidIcon v-if="isActive" />
          <OrderOutlineIcon v-else />
        </NavigationBarItem>

        <!-- Debt -->
        <NavigationBarItem name="debt" v-slot="{ isActive }">
          <DebtSolidIcon v-if="isActive" />
          <DebtOutlineIcon v-else />
        </NavigationBarItem>
        
        <!-- Expense -->
        <NavigationBarItem name="expense" v-slot="{ isActive }">
          <ExpenseSolidIcon v-if="isActive" />
          <ExpenseOutlineIcon v-else />
        </NavigationBarItem>

        <!-- Report -->
        <NavigationBarItem name="report" fallback-display-name="Báo cáo" v-slot="{ isActive }">
          <ReportSolidIcon v-if="isActive" />
          <ReportOutlineIcon v-else />
        </NavigationBarItem>

        <!-- Report -->
        <NavigationBarItem name="user" v-slot="{ isActive }">
          <UserSolidIcon v-if="isActive" />
          <UserOutlineIcon v-else />
        </NavigationBarItem>
      </div>
    </div>
  </nav>
</template>