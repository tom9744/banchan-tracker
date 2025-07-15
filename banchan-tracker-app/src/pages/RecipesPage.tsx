import { useState, useEffect } from "react";
import type {
  Banchan,
  CreateBanchanDto,
  UpdateBanchanDto,
} from "../types/banchan";
import { BanchanList } from "../components/BanchanList";
import { BanchanForm } from "../components/BanchanForm";
import { Button } from "../components/ui/Button";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "../components/ui/Card";
import { apiService } from "../services/api";

type ModalType = "create" | "edit" | null;

export function RecipesPage() {
  const [banchans, setBanchans] = useState<Banchan[]>([]);
  const [modalType, setModalType] = useState<ModalType>(null);
  const [editingBanchan, setEditingBanchan] = useState<Banchan | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingData, setIsLoadingData] = useState(true);

  // API에서 반찬 목록 가져오기
  const fetchBanchans = async () => {
    try {
      setIsLoadingData(true);
      const banchans = await apiService.getBanchans();
      setBanchans(banchans);
    } catch (error) {
      console.error("반찬 목록 가져오기 실패:", error);
      alert("반찬 목록을 가져오는데 실패했습니다.");
    } finally {
      setIsLoadingData(false);
    }
  };

  useEffect(() => {
    fetchBanchans();
  }, []);

  const handleCreate = () => {
    setModalType("create");
    setEditingBanchan(null);
  };

  const handleEdit = (banchan: Banchan) => {
    setModalType("edit");
    setEditingBanchan(banchan);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm("정말로 이 반찬을 삭제하시겠습니까?")) {
      try {
        await apiService.deleteBanchan(id);
        setBanchans(banchans.filter((banchan) => banchan.id !== id));
        alert("반찬이 성공적으로 삭제되었습니다.");
      } catch (error) {
        console.error("반찬 삭제 실패:", error);
        alert("반찬 삭제에 실패했습니다.");
      }
    }
  };

  const handleSubmit = async (data: CreateBanchanDto | UpdateBanchanDto) => {
    setIsLoading(true);

    try {
      if (modalType === "create") {
        // 새 반찬 추가
        const newBanchan = await apiService.createBanchan({
          name: (data as CreateBanchanDto).name,
        });
        setBanchans([...banchans, newBanchan]);
        alert(`"${newBanchan.name}" 반찬이 성공적으로 추가되었습니다.`);
      } else if (modalType === "edit" && editingBanchan) {
        // 반찬 수정
        const updatedBanchan = await apiService.updateBanchan(
          editingBanchan.id,
          { name: (data as UpdateBanchanDto).name }
        );
        const updatedBanchans = banchans.map((banchan) =>
          banchan.id === editingBanchan.id ? updatedBanchan : banchan
        );
        setBanchans(updatedBanchans);
        alert(
          `"${
            (data as UpdateBanchanDto).name
          }" 반찬이 성공적으로 수정되었습니다.`
        );
      }

      setModalType(null);
      setEditingBanchan(null);
    } catch (error) {
      console.error("반찬 처리 실패:", error);
      alert("반찬 처리에 실패했습니다.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = () => {
    setModalType(null);
    setEditingBanchan(null);
  };

  if (isLoadingData) {
    return (
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">반찬 목록을 불러오는 중...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-8">
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle>반찬 목록</CardTitle>
            <Button onClick={handleCreate}>새 반찬 추가</Button>
          </div>
        </CardHeader>
        <CardContent>
          <BanchanList
            banchans={banchans}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        </CardContent>
      </Card>

      {/* 모달 */}
      {modalType && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 w-full max-w-md mx-4">
            <BanchanForm
              banchan={editingBanchan || undefined}
              onSubmit={handleSubmit}
              onCancel={handleCancel}
              isLoading={isLoading}
            />
          </div>
        </div>
      )}
    </div>
  );
}
