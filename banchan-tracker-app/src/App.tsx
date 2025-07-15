import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Navigation } from "./components/Navigation";
import { RecipesPage } from "./pages/RecipesPage";
import "./App.css";

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <main>
          <Routes>
            <Route path="/recipes" element={<RecipesPage />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
